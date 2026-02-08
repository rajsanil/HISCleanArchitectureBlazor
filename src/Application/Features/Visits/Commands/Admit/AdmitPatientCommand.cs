using CleanArchitecture.Blazor.Application.Features.Visits.Caching;
using CleanArchitecture.Blazor.Application.Features.Beds.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Commands.Admit;

/// <summary>
/// Admits a patient for an existing visit. Creates an Admission record, assigns a bed, and updates visit/bed status.
/// </summary>
public class AdmitPatientCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int VisitId { get; set; }
    public int BedId { get; set; }
    public int RoomId { get; set; }
    public int LocationId { get; set; }
    public int? AdmittingDoctorId { get; set; }
    public string AdmissionType { get; set; } = "Elective";
    public DateTime? ExpectedDischargeDate { get; set; }

    public string CacheKey => VisitCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => VisitCacheKey.Tags?.Concat(BedCacheKey.Tags ?? Enumerable.Empty<string>());
}

public class AdmitPatientCommandValidator : AbstractValidator<AdmitPatientCommand>
{
    public AdmitPatientCommandValidator()
    {
        RuleFor(v => v.VisitId).GreaterThan(0).WithMessage("Visit is required.");
        RuleFor(v => v.BedId).GreaterThan(0).WithMessage("Bed is required.");
        RuleFor(v => v.RoomId).GreaterThan(0).WithMessage("Room is required.");
        RuleFor(v => v.LocationId).GreaterThan(0).WithMessage("Location is required.");
        RuleFor(v => v.AdmissionType).MaximumLength(50).NotEmpty();
    }
}

public class AdmitPatientCommandHandler : IRequestHandler<AdmitPatientCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public AdmitPatientCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AdmitPatientCommand request, CancellationToken cancellationToken)
    {
        var visit = await _context.Visits
            .Include(v => v.Admission)
            .SingleOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);
        if (visit == null)
            return await Result<int>.FailureAsync($"Visit with id: [{request.VisitId}] not found.");
        if (visit.Admission != null)
            return await Result<int>.FailureAsync("Patient is already admitted for this visit.");
        if (visit.VisitStatus == "Discharged" || visit.VisitStatus == "Cancelled")
            return await Result<int>.FailureAsync($"Cannot admit patient. Visit status is '{visit.VisitStatus}'.");

        // Verify bed availability
        var bed = await _context.Beds.SingleOrDefaultAsync(b => b.Id == request.BedId, cancellationToken);
        if (bed == null)
            return await Result<int>.FailureAsync($"Bed with id: [{request.BedId}] not found.");
        if (bed.BedStatus != "Available")
            return await Result<int>.FailureAsync($"Bed '{bed.Code}' is not available. Current status: {bed.BedStatus}");

        // Create admission record
        var admission = new Admission
        {
            VisitId = visit.Id,
            AdmissionDate = DateTime.UtcNow,
            BedId = request.BedId,
            RoomId = request.RoomId,
            LocationId = request.LocationId,
            AdmittingDoctorId = request.AdmittingDoctorId,
            AdmissionType = request.AdmissionType,
            ExpectedDischargeDate = request.ExpectedDischargeDate,
            TenantId = visit.TenantId
        };
        admission.AddDomainEvent(new CreatedEvent<Admission>(admission));
        _context.Admissions.Add(admission);

        // Update bed status
        bed.BedStatus = "Occupied";
        bed.AddDomainEvent(new UpdatedEvent<Bed>(bed));

        // Update visit status
        visit.VisitStatus = "Admitted";
        visit.AddDomainEvent(new UpdatedEvent<Visit>(visit));

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(admission.Id);
    }
}
