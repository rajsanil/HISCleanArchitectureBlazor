using CleanArchitecture.Blazor.Application.Features.StaffMembers.Caching;

namespace CleanArchitecture.Blazor.Application.Features.StaffMembers.Commands.Delete;

public class DeleteStaffCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteStaffCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => StaffCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => StaffCacheKey.Tags;
}

public class DeleteStaffCommandHandler : IRequestHandler<DeleteStaffCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public DeleteStaffCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Staff.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Staff>(item));
            _context.Staff.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
