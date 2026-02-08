using CleanArchitecture.Blazor.Application.Features.Facilities.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Facilities.Commands.Delete;

public class DeleteFacilityCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteFacilityCommand(int[] id) { Id = id; }
    public int[] Id { get; }
    public string CacheKey => FacilityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => FacilityCacheKey.Tags;
}

public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public DeleteFacilityCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Facilities.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Facility>(item));
            _context.Facilities.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
