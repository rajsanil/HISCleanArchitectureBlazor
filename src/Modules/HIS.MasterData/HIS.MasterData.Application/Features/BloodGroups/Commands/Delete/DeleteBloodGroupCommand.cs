using HIS.MasterData.Application.Features.BloodGroups.Caching;

namespace HIS.MasterData.Application.Features.BloodGroups.Commands.Delete;

public class DeleteBloodGroupCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteBloodGroupCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string CacheKey => BloodGroupCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BloodGroupCacheKey.Tags;
}

public class DeleteBloodGroupCommandHandler : IRequestHandler<DeleteBloodGroupCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public DeleteBloodGroupCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteBloodGroupCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.BloodGroups
            .Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<BloodGroup>(item));
            _context.BloodGroups.Remove(item);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
