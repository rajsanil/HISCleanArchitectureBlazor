using HIS.MasterData.Application.Features.Cities.Caching;

namespace HIS.MasterData.Application.Features.Cities.Commands.Delete;

public class DeleteCityCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteCityCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string CacheKey => CityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CityCacheKey.Tags;
}

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public DeleteCityCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Cities
            .Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<City>(item));
            _context.Cities.Remove(item);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
