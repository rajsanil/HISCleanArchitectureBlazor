using HIS.MasterData.Application.Features.BloodGroups.Caching;
using HIS.MasterData.Application.Features.BloodGroups.DTOs;
using HIS.MasterData.Application.Features.BloodGroups.Mappers;

namespace HIS.MasterData.Application.Features.BloodGroups.Queries.GetAll;

public class GetAllBloodGroupsQuery : ICacheableRequest<IEnumerable<BloodGroupDto>>
{
    public string CacheKey => BloodGroupCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BloodGroupCacheKey.Tags;
}

public class GetAllBloodGroupsQueryHandler : IRequestHandler<GetAllBloodGroupsQuery, IEnumerable<BloodGroupDto>>
{
    private readonly IMasterDataDbContext _context;

    public GetAllBloodGroupsQueryHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BloodGroupDto>> Handle(GetAllBloodGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _context.BloodGroups.OrderBy(x => x.DisplayOrder).ProjectTo().ToListAsync(cancellationToken);
    }
}
