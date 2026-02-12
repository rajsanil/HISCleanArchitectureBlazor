using HIS.MasterData.Application.Features.Departments.Caching;
using HIS.MasterData.Application.Features.Departments.DTOs;
using HIS.MasterData.Application.Features.Departments.Mappers;

namespace HIS.MasterData.Application.Features.Departments.Queries.GetAll;

public class GetAllDepartmentsQuery : ICacheableRequest<IEnumerable<DepartmentDto>>
{
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => DepartmentCacheKey.Tags;
}

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentDto>>
{
    private readonly IMasterDataDbContext _context;
    public GetAllDepartmentsQueryHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Departments.ProjectTo().ToListAsync(cancellationToken);
    }
}
