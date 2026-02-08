using CleanArchitecture.Blazor.Application.Features.MaritalStatuses.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Queries.GetAll;

public class GetAllMaritalStatusesQuery : IRequest<IEnumerable<MaritalStatusDto>>
{
}

public class GetAllMaritalStatusesQueryHandler : IRequestHandler<GetAllMaritalStatusesQuery, IEnumerable<MaritalStatusDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMaritalStatusesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MaritalStatusDto>> Handle(GetAllMaritalStatusesQuery request, CancellationToken cancellationToken)
    {
        return await _context.MaritalStatuses
            .Where(x => x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .Select(x => new MaritalStatusDto
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
