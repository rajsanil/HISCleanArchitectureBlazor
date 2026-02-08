using CleanArchitecture.Blazor.Application.Features.Locations.Caching;
using CleanArchitecture.Blazor.Application.Features.Locations.DTOs;
using CleanArchitecture.Blazor.Application.Features.Locations.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Locations.Queries.GetAll;

public class GetAllLocationsQuery : ICacheableRequest<IEnumerable<LocationDto>>
{
    public string CacheKey => LocationCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => LocationCacheKey.Tags;
}

public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllLocationsQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Locations.ProjectTo().ToListAsync(cancellationToken);
    }
}
