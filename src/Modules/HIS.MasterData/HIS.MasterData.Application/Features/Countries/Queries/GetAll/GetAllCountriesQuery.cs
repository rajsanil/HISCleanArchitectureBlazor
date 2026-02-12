using HIS.MasterData.Application.Features.Countries.Caching;
using HIS.MasterData.Application.Features.Countries.DTOs;
using HIS.MasterData.Application.Features.Countries.Mappers;

namespace HIS.MasterData.Application.Features.Countries.Queries.GetAll;

public class GetAllCountriesQuery : ICacheableRequest<IEnumerable<CountryDto>>
{
    public string CacheKey => CountryCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CountryCacheKey.Tags;
}

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
{
    private readonly IMasterDataDbContext _context;

    public GetAllCountriesQueryHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries.ProjectTo().ToListAsync(cancellationToken);
    }
}
