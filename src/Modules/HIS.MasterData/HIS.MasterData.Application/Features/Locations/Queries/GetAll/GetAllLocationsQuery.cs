using HIS.MasterData.Application.Features.Locations.Caching;
using HIS.MasterData.Application.Features.Locations.DTOs;
using HIS.MasterData.Application.Features.Locations.Mappers;

namespace HIS.MasterData.Application.Features.Locations.Queries.GetAll;

public class GetAllLocationsQuery : ICacheableRequest<IEnumerable<LocationDto>>
{
    public string CacheKey => LocationCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => LocationCacheKey.Tags;
}

public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDto>>
{
    private readonly IMasterDataDbContext _context;
    public GetAllLocationsQueryHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<IEnumerable<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Locations.ProjectTo().ToListAsync(cancellationToken);
    }
}
