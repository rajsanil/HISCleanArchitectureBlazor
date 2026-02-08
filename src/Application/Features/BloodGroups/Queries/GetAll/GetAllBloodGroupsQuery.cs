using CleanArchitecture.Blazor.Application.Features.BloodGroups.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.Queries.GetAll;

public class GetAllBloodGroupsQuery : IRequest<IEnumerable<BloodGroupDto>>
{
}

public class GetAllBloodGroupsQueryHandler : IRequestHandler<GetAllBloodGroupsQuery, IEnumerable<BloodGroupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllBloodGroupsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BloodGroupDto>> Handle(GetAllBloodGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _context.BloodGroups
            .Where(x => x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .Select(x => new BloodGroupDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                NameArabic = x.NameArabic,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
