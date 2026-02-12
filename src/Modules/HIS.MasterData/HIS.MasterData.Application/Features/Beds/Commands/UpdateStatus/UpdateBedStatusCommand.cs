using HIS.MasterData.Application.Features.Beds.Caching;

namespace HIS.MasterData.Application.Features.Beds.Commands.UpdateStatus;

/// <summary>
/// Updates the status of a specific bed (e.g., Available, Occupied, Maintenance, Cleaning).
/// </summary>
public class UpdateBedStatusCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int BedId { get; set; }
    public string NewStatus { get; set; } = string.Empty;

    public string CacheKey => BedCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BedCacheKey.Tags;
}

public class UpdateBedStatusCommandValidator : AbstractValidator<UpdateBedStatusCommand>
{
    private static readonly string[] AllowedStatuses = { "Available", "Occupied", "Maintenance", "Cleaning", "Reserved", "Blocked" };
    
    public UpdateBedStatusCommandValidator()
    {
        RuleFor(v => v.BedId).GreaterThan(0);
        RuleFor(v => v.NewStatus).NotEmpty().Must(s => AllowedStatuses.Contains(s))
            .WithMessage($"Status must be one of: {string.Join(", ", AllowedStatuses)}");
    }
}

public class UpdateBedStatusCommandHandler : IRequestHandler<UpdateBedStatusCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;
    public UpdateBedStatusCommandHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(UpdateBedStatusCommand request, CancellationToken cancellationToken)
    {
        var bed = await _context.Beds.SingleOrDefaultAsync(x => x.Id == request.BedId, cancellationToken);
        if (bed == null)
            return await Result<int>.FailureAsync($"Bed with id: [{request.BedId}] not found.");
        
        bed.BedStatus = request.NewStatus;
        bed.AddDomainEvent(new UpdatedEvent<Bed>(bed));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(bed.Id);
    }
}
