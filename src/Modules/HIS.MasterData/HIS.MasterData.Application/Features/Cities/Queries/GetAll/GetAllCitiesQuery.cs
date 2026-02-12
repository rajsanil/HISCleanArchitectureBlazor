using HIS.MasterData.Application.Features.Cities.Caching;
using HIS.MasterData.Application.Features.Cities.DTOs;
using HIS.MasterData.Application.Features.Cities.Mappers;

namespace HIS.MasterData.Application.Features.Cities.Queries.GetAll;

public class GetAllCitiesQuery : ICacheableRequest<IEnumerable<CityDto>>
{
    public string CacheKey => CityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CityCacheKey.Tags;
}

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
{
    private readonly IMasterDataDbContext _context;

    public GetAllCitiesQueryHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Cities.OrderBy(x => x.Name).ProjectTo().ToListAsync(cancellationToken);
    }
}
