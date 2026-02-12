using HIS.MasterData.Application.Features.Beds.Caching;

namespace HIS.MasterData.Application.Features.Beds.Commands.Delete;

public class DeleteBedCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteBedCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => BedCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BedCacheKey.Tags;
}

public class DeleteBedCommandHandler : IRequestHandler<DeleteBedCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;
    public DeleteBedCommandHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteBedCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Beds.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Bed>(item));
            _context.Beds.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
