using HIS.MasterData.Application.Features.Departments.Caching;

namespace HIS.MasterData.Application.Features.Departments.Commands.Delete;

public class DeleteDepartmentCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteDepartmentCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => DepartmentCacheKey.Tags;
}

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;
    public DeleteDepartmentCommandHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Departments.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Department>(item));
            _context.Departments.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
