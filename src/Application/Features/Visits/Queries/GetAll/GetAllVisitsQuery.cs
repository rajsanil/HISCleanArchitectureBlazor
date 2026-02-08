using CleanArchitecture.Blazor.Application.Features.Visits.Caching;
using CleanArchitecture.Blazor.Application.Features.Visits.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visits.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Queries.GetAll;

public class GetAllVisitsQuery : ICacheableRequest<IEnumerable<VisitDto>>
{
    public string CacheKey => VisitCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => VisitCacheKey.Tags;
}

public class GetByIdVisitQuery : ICacheableRequest<VisitDto?>
{
    public required int Id { get; set; }
    public string CacheKey => VisitCacheKey.GetVisitByIdCacheKey(Id);
    public IEnumerable<string>? Tags => VisitCacheKey.Tags;
}

public class GetAllVisitsQueryHandler : IRequestHandler<GetAllVisitsQuery, IEnumerable<VisitDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllVisitsQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<VisitDto>> Handle(GetAllVisitsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Visits.OrderByDescending(v => v.RegistrationDate).ProjectTo().ToListAsync(cancellationToken);
    }
}

public class GetByIdVisitQueryHandler : IRequestHandler<GetByIdVisitQuery, VisitDto?>
{
    private readonly IApplicationDbContext _context;
    public GetByIdVisitQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<VisitDto?> Handle(GetByIdVisitQuery request, CancellationToken cancellationToken)
    {
        return await _context.Visits.Where(x => x.Id == request.Id).ProjectTo().SingleOrDefaultAsync(cancellationToken);
    }
}
