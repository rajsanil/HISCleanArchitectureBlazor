using HIS.MasterData.Application.Features.Beds.Caching;
using HIS.MasterData.Application.Features.Beds.DTOs;
using HIS.MasterData.Application.Features.Beds.Mappers;

namespace HIS.MasterData.Application.Features.Beds.Queries.GetAll;

public class GetAllBedsQuery : ICacheableRequest<IEnumerable<BedDto>>
{
    public string CacheKey => BedCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BedCacheKey.Tags;
}

public class GetAllBedsQueryHandler : IRequestHandler<GetAllBedsQuery, IEnumerable<BedDto>>
{
    private readonly IMasterDataDbContext _context;
    public GetAllBedsQueryHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<IEnumerable<BedDto>> Handle(GetAllBedsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Beds.ProjectTo().ToListAsync(cancellationToken);
    }
}
