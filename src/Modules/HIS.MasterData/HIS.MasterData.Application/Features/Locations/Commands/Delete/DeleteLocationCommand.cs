using HIS.MasterData.Application.Features.Locations.Caching;

namespace HIS.MasterData.Application.Features.Locations.Commands.Delete;

public class DeleteLocationCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteLocationCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => LocationCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => LocationCacheKey.Tags;
}

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;
    public DeleteLocationCommandHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Locations.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Location>(item));
            _context.Locations.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
