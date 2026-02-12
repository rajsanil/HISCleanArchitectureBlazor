using CleanArchitecture.Blazor.Application.Features.Visits.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Commands.Cancel;

/// <summary>
/// Cancels a visit. Releases assigned bed if admitted.
/// </summary>
public class CancelVisitCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int VisitId { get; set; }
    public string CancellationReason { get; set; } = string.Empty;

    public string CacheKey => VisitCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => VisitCacheKey.Tags; // TODO: Add BedCacheKey.Tags when cross-module caching is implemented
}

public class CancelVisitCommandValidator : AbstractValidator<CancelVisitCommand>
{
    public CancelVisitCommandValidator()
    {
        RuleFor(v => v.VisitId).GreaterThan(0);
        RuleFor(v => v.CancellationReason).MaximumLength(500).NotEmpty().WithMessage("Cancellation reason is required.");
    }
}

public class CancelVisitCommandHandler : IRequestHandler<CancelVisitCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CancelVisitCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(CancelVisitCommand request, CancellationToken cancellationToken)
    {
        var visit = await _context.Visits
            .Include(v => v.Admission)
            .SingleOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);
        if (visit == null)
            return await Result<int>.FailureAsync($"Visit with id: [{request.VisitId}] not found.");
        if (visit.VisitStatus == "Discharged")
            return await Result<int>.FailureAsync("Cannot cancel a discharged visit.");
        if (visit.VisitStatus == "Cancelled")
            return await Result<int>.FailureAsync("Visit is already cancelled.");

        // TODO: Release bed if admitted - requires cross-module communication with MasterData module
        // if (visit.Admission != null)
        // {
        //     var bed = await _context.Beds.SingleOrDefaultAsync(b => b.Id == visit.Admission.BedId, cancellationToken);
        //     if (bed != null)
        //     {
        //         bed.BedStatus = "Available";
        //         bed.AddDomainEvent(new UpdatedEvent<Bed>(bed));
        //     }
        // }

        visit.VisitStatus = "Cancelled";
        visit.AddDomainEvent(new UpdatedEvent<Visit>(visit));

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(visit.Id);
    }
}
