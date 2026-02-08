#nullable disable

using CleanArchitecture.Blazor.Application.Features.Cities.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Cities.Commands.Delete;

public class DeleteCityCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
    public string CacheKey => CityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CityCacheKey.Tags;
    
    public DeleteCityCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteCityCommandHandler(IApplicationDbContext context)
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
            item.AddDomainEvent(new CityDeletedEvent(item));
            _context.Cities.Remove(item);
        }
        
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
