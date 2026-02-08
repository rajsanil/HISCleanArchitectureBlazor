#nullable disable

using CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Caching;

namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Commands.Delete;

public class DeleteMaritalStatusCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
    public string CacheKey => MaritalStatusCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => MaritalStatusCacheKey.Tags;
    
    public DeleteMaritalStatusCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteMaritalStatusCommandHandler : IRequestHandler<DeleteMaritalStatusCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteMaritalStatusCommandHandler(IApplicationDbContext context)
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
            item.AddDomainEvent(new MaritalStatusDeletedEvent(item));
            _context.MaritalStatuses.Remove(item);
        }
        
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
