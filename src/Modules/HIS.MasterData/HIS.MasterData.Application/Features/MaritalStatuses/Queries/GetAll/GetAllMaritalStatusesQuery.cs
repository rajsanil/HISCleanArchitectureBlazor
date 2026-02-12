using HIS.MasterData.Application.Features.MaritalStatuses.Caching;
using HIS.MasterData.Application.Features.MaritalStatuses.DTOs;
using HIS.MasterData.Application.Features.MaritalStatuses.Mappers;

namespace HIS.MasterData.Application.Features.MaritalStatuses.Queries.GetAll;

public class GetAllMaritalStatusesQuery : ICacheableRequest<IEnumerable<MaritalStatusDto>>
{
    public string CacheKey => MaritalStatusCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => MaritalStatusCacheKey.Tags;
}

public class GetAllMaritalStatusesQueryHandler : IRequestHandler<GetAllMaritalStatusesQuery, IEnumerable<MaritalStatusDto>>
{
    private readonly IMasterDataDbContext _context;

    public GetAllMaritalStatusesQueryHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MaritalStatusDto>> Handle(GetAllMaritalStatusesQuery request, CancellationToken cancellationToken)
    {
        return await _context.MaritalStatuses.OrderBy(x => x.DisplayOrder).ProjectTo().ToListAsync(cancellationToken);
    }
}
