using CleanArchitecture.Blazor.Application.Features.Locations.Caching;
using CleanArchitecture.Blazor.Application.Features.Locations.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Locations.Commands.AddEdit;

public class AddEditLocationCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LocationType { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int FacilityId { get; set; }
    public bool IsActive { get; set; } = true;

    public string CacheKey => LocationCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => LocationCacheKey.Tags;
}

public class AddEditLocationCommandHandler : IRequestHandler<AddEditLocationCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public AddEditLocationCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Result<int>> Handle(AddEditLocationCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Locations.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null)
                return await Result<int>.FailureAsync($"Location with id: [{request.Id}] not found.");
            LocationMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new UpdatedEvent<Location>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = LocationMapper.FromEditCommand(request);
            item.AddDomainEvent(new CreatedEvent<Location>(item));
            _context.Locations.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
