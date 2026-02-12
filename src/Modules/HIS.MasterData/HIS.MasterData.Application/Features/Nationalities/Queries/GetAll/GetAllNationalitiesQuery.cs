using HIS.MasterData.Application.Features.Nationalities.Caching;
using HIS.MasterData.Application.Features.Nationalities.DTOs;
using HIS.MasterData.Application.Features.Nationalities.Mappers;

namespace HIS.MasterData.Application.Features.Nationalities.Queries.GetAll;

public class GetAllNationalitiesQuery : ICacheableRequest<IEnumerable<NationalityDto>>
{
    public string CacheKey => NationalityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => NationalityCacheKey.Tags;
}

public class GetAllNationalitiesQueryHandler : IRequestHandler<GetAllNationalitiesQuery, IEnumerable<NationalityDto>>
{
    private readonly IMasterDataDbContext _context;

    public GetAllNationalitiesQueryHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NationalityDto>> Handle(GetAllNationalitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Nationalities.OrderBy(x => x.Name).ProjectTo().ToListAsync(cancellationToken);
    }
}
