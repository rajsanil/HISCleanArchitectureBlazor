#nullable disable

using CleanArchitecture.Blazor.Application.Features.Nationalities.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Nationalities.Commands.Delete;

public class DeleteNationalityCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
    public string CacheKey => NationalityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => NationalityCacheKey.Tags;
    
    public DeleteNationalityCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteNationalityCommandHandler : IRequestHandler<DeleteNationalityCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteNationalityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<int>> Handle(DeleteNationalityCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Nationalities
            .Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);
            
        foreach (var item in items)
        {
            item.AddDomainEvent(new NationalityDeletedEvent(item));
            _context.Nationalities.Remove(item);
        }
        
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
