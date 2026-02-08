using CleanArchitecture.Blazor.Application.Features.Beds.Caching;
using CleanArchitecture.Blazor.Application.Features.Beds.DTOs;
using CleanArchitecture.Blazor.Application.Features.Beds.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Beds.Queries.GetAll;

public class GetAllBedsQuery : ICacheableRequest<IEnumerable<BedDto>>
{
    public string CacheKey => BedCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BedCacheKey.Tags;
}

public class GetAllBedsQueryHandler : IRequestHandler<GetAllBedsQuery, IEnumerable<BedDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllBedsQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<BedDto>> Handle(GetAllBedsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Beds.ProjectTo().ToListAsync(cancellationToken);
    }
}
