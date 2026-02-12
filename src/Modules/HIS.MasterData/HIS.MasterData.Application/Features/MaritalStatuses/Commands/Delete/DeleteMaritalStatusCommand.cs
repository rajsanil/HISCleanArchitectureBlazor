using HIS.MasterData.Application.Features.MaritalStatuses.Caching;

namespace HIS.MasterData.Application.Features.MaritalStatuses.Commands.Delete;

public class DeleteMaritalStatusCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteMaritalStatusCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string CacheKey => MaritalStatusCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => MaritalStatusCacheKey.Tags;
}

public class DeleteMaritalStatusCommandHandler : IRequestHandler<DeleteMaritalStatusCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public DeleteMaritalStatusCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteMaritalStatusCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.MaritalStatuses
            .Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<MaritalStatus>(item));
            _context.MaritalStatuses.Remove(item);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
