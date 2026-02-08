using CleanArchitecture.Blazor.Application.Features.Visits.Caching;
using CleanArchitecture.Blazor.Application.Features.Visits.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visits.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Visits.Queries.Pagination;

public class VisitsWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<VisitDto>>
{
    public string? VisitType { get; set; }
    public string? VisitStatus { get; set; }
    public int? PatientId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public override string ToString()
    {
        return $"Search:{Keyword},VisitType:{VisitType},VisitStatus:{VisitStatus},PatientId:{PatientId},From:{FromDate},To:{ToDate},{OrderBy},{SortDirection},{PageNumber},{PageSize}";
    }

    public string CacheKey => VisitCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => VisitCacheKey.Tags;
}

public class VisitsWithPaginationQueryHandler : IRequestHandler<VisitsWithPaginationQuery, PaginatedData<VisitDto>>
{
    private readonly IApplicationDbContext _context;
    public VisitsWithPaginationQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<PaginatedData<VisitDto>> Handle(VisitsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Visits.AsQueryable();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(v => v.VisitNumber.Contains(request.Keyword));
        }
        if (!string.IsNullOrEmpty(request.VisitType))
        {
            query = query.Where(v => v.VisitType == request.VisitType);
        }
        if (!string.IsNullOrEmpty(request.VisitStatus))
        {
            query = query.Where(v => v.VisitStatus == request.VisitStatus);
        }
        if (request.PatientId.HasValue)
        {
            query = query.Where(v => v.PatientId == request.PatientId.Value);
        }
        if (request.FromDate.HasValue)
        {
            query = query.Where(v => v.RegistrationDate >= request.FromDate.Value);
        }
        if (request.ToDate.HasValue)
        {
            query = query.Where(v => v.RegistrationDate <= request.ToDate.Value);
        }

        var ordered = query.OrderByDescending(v => v.RegistrationDate).ProjectTo();
        return await PaginatedData<VisitDto>.CreateAsync(ordered, request.PageNumber, request.PageSize);
    }
}
