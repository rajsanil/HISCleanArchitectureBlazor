using CleanArchitecture.Blazor.Application.Features.Facilities.Caching;
using CleanArchitecture.Blazor.Application.Features.Facilities.DTOs;
using CleanArchitecture.Blazor.Application.Features.Facilities.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Facilities.Queries.GetAll;

public class GetAllFacilitiesQuery : ICacheableRequest<IEnumerable<FacilityDto>>
{
    public string CacheKey => FacilityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => FacilityCacheKey.Tags;
}

public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, IEnumerable<FacilityDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllFacilitiesQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<FacilityDto>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Facilities.ProjectTo().ToListAsync(cancellationToken);
    }
}
