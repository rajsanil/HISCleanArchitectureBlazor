using HIS.MasterData.Application.Features.Countries.Caching;

namespace HIS.MasterData.Application.Features.Countries.Commands.Delete;

public class DeleteCountryCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteCountryCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string CacheKey => CountryCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CountryCacheKey.Tags;
}

public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public DeleteCountryCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Countries
            .Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Country>(item));
            _context.Countries.Remove(item);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
