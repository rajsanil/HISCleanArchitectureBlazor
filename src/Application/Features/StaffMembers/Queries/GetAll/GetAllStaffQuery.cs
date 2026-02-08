using CleanArchitecture.Blazor.Application.Features.StaffMembers.Caching;
using CleanArchitecture.Blazor.Application.Features.StaffMembers.DTOs;
using CleanArchitecture.Blazor.Application.Features.StaffMembers.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.StaffMembers.Queries.GetAll;

public class GetAllStaffQuery : ICacheableRequest<IEnumerable<StaffDto>>
{
    public string CacheKey => StaffCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => StaffCacheKey.Tags;
}

public class GetAllStaffQueryHandler : IRequestHandler<GetAllStaffQuery, IEnumerable<StaffDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllStaffQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<StaffDto>> Handle(GetAllStaffQuery request, CancellationToken cancellationToken)
    {
        return await _context.Staff.ProjectTo().ToListAsync(cancellationToken);
    }
}
