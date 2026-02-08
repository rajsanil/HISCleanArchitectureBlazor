using CleanArchitecture.Blazor.Application.Features.Encounters.Caching;
using CleanArchitecture.Blazor.Application.Features.Encounters.DTOs;
using CleanArchitecture.Blazor.Application.Features.Encounters.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Encounters.Queries.GetAll;

public class GetAllEncountersQuery : ICacheableRequest<IEnumerable<EncounterDto>>
{
    public string CacheKey => EncounterCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => EncounterCacheKey.Tags;
}

public class GetByVisitIdEncountersQuery : IRequest<IEnumerable<EncounterDto>>
{
    public required int VisitId { get; set; }
}

public class GetAllEncountersQueryHandler : IRequestHandler<GetAllEncountersQuery, IEnumerable<EncounterDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllEncountersQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<EncounterDto>> Handle(GetAllEncountersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Encounters.OrderByDescending(e => e.StartDate).ProjectTo().ToListAsync(cancellationToken);
    }
}

public class GetByVisitIdEncountersQueryHandler : IRequestHandler<GetByVisitIdEncountersQuery, IEnumerable<EncounterDto>>
{
    private readonly IApplicationDbContext _context;
    public GetByVisitIdEncountersQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<EncounterDto>> Handle(GetByVisitIdEncountersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Encounters
            .Where(e => e.VisitId == request.VisitId)
            .OrderByDescending(e => e.StartDate)
            .ProjectTo()
            .ToListAsync(cancellationToken);
    }
}
