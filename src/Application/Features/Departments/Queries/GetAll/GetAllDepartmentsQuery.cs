using CleanArchitecture.Blazor.Application.Features.Departments.Caching;
using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Queries.GetAll;

public class GetAllDepartmentsQuery : ICacheableRequest<IEnumerable<DepartmentDto>>
{
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => DepartmentCacheKey.Tags;
}

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllDepartmentsQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Departments.ProjectTo().ToListAsync(cancellationToken);
    }
}
