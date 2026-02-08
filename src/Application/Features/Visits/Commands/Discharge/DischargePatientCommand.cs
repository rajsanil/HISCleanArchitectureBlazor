using CleanArchitecture.Blazor.Application.Features.Visits.Caching;
using CleanArchitecture.Blazor.Application.Features.Beds.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Commands.Discharge;

/// <summary>
/// Discharges an admitted patient. Creates Discharge record, releases bed, and updates visit status.
/// </summary>
public class DischargePatientCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int VisitId { get; set; }
    public string DischargeDisposition { get; set; } = string.Empty;
    public string? DischargeSummary { get; set; }
    public int? DischargedByDoctorId { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public string? FollowUpNotes { get; set; }

    public string CacheKey => VisitCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => VisitCacheKey.Tags?.Concat(BedCacheKey.Tags ?? Enumerable.Empty<string>());
}

public class DischargePatientCommandValidator : AbstractValidator<DischargePatientCommand>
{
    public DischargePatientCommandValidator()
    {
        RuleFor(v => v.VisitId).GreaterThan(0).WithMessage("Visit is required.");
        RuleFor(v => v.DischargeDisposition).MaximumLength(100).NotEmpty().WithMessage("Discharge disposition is required.");
    }
}

public class DischargePatientCommandHandler : IRequestHandler<DischargePatientCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public DischargePatientCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DischargePatientCommand request, CancellationToken cancellationToken)
    {
        var visit = await _context.Visits
            .Include(v => v.Admission)
            .Include(v => v.Discharge)
            .SingleOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);
        if (visit == null)
            return await Result<int>.FailureAsync($"Visit with id: [{request.VisitId}] not found.");
        if (visit.Discharge != null)
            return await Result<int>.FailureAsync("Patient has already been discharged for this visit.");
        if (visit.VisitStatus == "Cancelled")
            return await Result<int>.FailureAsync("Cannot discharge a cancelled visit.");

        // Create discharge record
        var discharge = new Domain.Entities.Discharge
        {
            VisitId = visit.Id,
            DischargeDate = DateTime.UtcNow,
            DischargeDisposition = request.DischargeDisposition,
            DischargeSummary = request.DischargeSummary,
            DischargedByDoctorId = request.DischargedByDoctorId,
            FollowUpDate = request.FollowUpDate,
            FollowUpNotes = request.FollowUpNotes,
            TenantId = visit.TenantId
        };
        discharge.AddDomainEvent(new CreatedEvent<Domain.Entities.Discharge>(discharge));
        _context.Discharges.Add(discharge);

        // Release bed if admitted
        if (visit.Admission != null)
        {
            var bed = await _context.Beds.SingleOrDefaultAsync(b => b.Id == visit.Admission.BedId, cancellationToken);
            if (bed != null)
            {
                bed.BedStatus = "Cleaning";
                bed.AddDomainEvent(new UpdatedEvent<Bed>(bed));
            }
        }

        // Update visit
        visit.VisitStatus = "Discharged";
        visit.DischargeDate = DateTime.UtcNow;
        visit.DischargeDisposition = request.DischargeDisposition;
        visit.AddDomainEvent(new UpdatedEvent<Visit>(visit));

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(discharge.Id);
    }
}
