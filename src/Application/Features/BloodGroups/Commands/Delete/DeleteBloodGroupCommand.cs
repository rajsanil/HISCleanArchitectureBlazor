#nullable disable

using CleanArchitecture.Blazor.Application.Features.BloodGroups.Caching;

namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.Commands.Delete;

public class DeleteBloodGroupCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
    public string CacheKey => BloodGroupCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BloodGroupCacheKey.Tags;
    
    public DeleteBloodGroupCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteBloodGroupCommandHandler : IRequestHandler<DeleteBloodGroupCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteBloodGroupCommandHandler(IApplicationDbContext context)
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
            item.AddDomainEvent(new BloodGroupDeletedEvent(item));
            _context.BloodGroups.Remove(item);
        }
        
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
