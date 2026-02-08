using CleanArchitecture.Blazor.Application.Features.Visits.Caching;
using CleanArchitecture.Blazor.Application.Features.Beds.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Commands.Transfer;

/// <summary>
/// Transfers an admitted patient from one bed/location to another.
/// Updates bed statuses (old bed → Available, new bed → Occupied) and creates a Transfer record.
/// </summary>
public class TransferPatientCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int VisitId { get; set; }
    public int ToBedId { get; set; }
    public int ToLocationId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public int? OrderedByDoctorId { get; set; }

    public string CacheKey => VisitCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => VisitCacheKey.Tags?.Concat(BedCacheKey.Tags ?? Enumerable.Empty<string>());
}

public class TransferPatientCommandValidator : AbstractValidator<TransferPatientCommand>
{
    public TransferPatientCommandValidator()
    {
        RuleFor(v => v.VisitId).GreaterThan(0).WithMessage("Visit is required.");
        RuleFor(v => v.ToBedId).GreaterThan(0).WithMessage("Destination bed is required.");
        RuleFor(v => v.ToLocationId).GreaterThan(0).WithMessage("Destination location is required.");
        RuleFor(v => v.Reason).MaximumLength(500).NotEmpty().WithMessage("Reason for transfer is required.");
    }
}

public class TransferPatientCommandHandler : IRequestHandler<TransferPatientCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public TransferPatientCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(TransferPatientCommand request, CancellationToken cancellationToken)
    {
        var visit = await _context.Visits
            .Include(v => v.Admission)
            .SingleOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);
        if (visit == null)
            return await Result<int>.FailureAsync($"Visit with id: [{request.VisitId}] not found.");
        if (visit.Admission == null)
            return await Result<int>.FailureAsync("Patient is not admitted. Cannot transfer.");
        if (visit.VisitStatus != "Admitted")
            return await Result<int>.FailureAsync($"Cannot transfer patient. Visit status is '{visit.VisitStatus}'.");

        // Verify destination bed availability
        var toBed = await _context.Beds.SingleOrDefaultAsync(b => b.Id == request.ToBedId, cancellationToken);
        if (toBed == null)
            return await Result<int>.FailureAsync($"Destination bed with id: [{request.ToBedId}] not found.");
        if (toBed.BedStatus != "Available")
            return await Result<int>.FailureAsync($"Destination bed '{toBed.Code}' is not available. Current status: {toBed.BedStatus}");

        // Release current bed
        var fromBed = await _context.Beds.SingleOrDefaultAsync(b => b.Id == visit.Admission.BedId, cancellationToken);
        if (fromBed != null)
        {
            fromBed.BedStatus = "Cleaning";
            fromBed.AddDomainEvent(new UpdatedEvent<Bed>(fromBed));
        }

        // Create transfer record
        var transfer = new Domain.Entities.Transfer
        {
            VisitId = visit.Id,
            FromBedId = visit.Admission.BedId,
            ToBedId = request.ToBedId,
            FromLocationId = visit.Admission.LocationId,
            ToLocationId = request.ToLocationId,
            TransferDate = DateTime.UtcNow,
            Reason = request.Reason,
            OrderedByDoctorId = request.OrderedByDoctorId,
            TenantId = visit.TenantId
        };
        transfer.AddDomainEvent(new CreatedEvent<Domain.Entities.Transfer>(transfer));
        _context.Transfers.Add(transfer);

        // Update admission to new bed/location
        visit.Admission.BedId = request.ToBedId;
        visit.Admission.LocationId = request.ToLocationId;

        // Occupy new bed
        toBed.BedStatus = "Occupied";
        toBed.AddDomainEvent(new UpdatedEvent<Bed>(toBed));

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(transfer.Id);
    }
}
