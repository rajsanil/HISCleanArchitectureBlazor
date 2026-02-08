using CleanArchitecture.Blazor.Application.Features.Cities.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Cities.Queries.GetAll;

public class GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>
{
}

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCitiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Cities
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new CityDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                NameArabic = x.NameArabic,
                CountryId = x.CountryId,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
