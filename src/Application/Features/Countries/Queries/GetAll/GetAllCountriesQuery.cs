using CleanArchitecture.Blazor.Application.Features.Countries.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Countries.Queries.GetAll;

public class GetAllCountriesQuery : IRequest<IEnumerable<CountryDto>>
{
}

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCountriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new CountryDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                NameArabic = x.NameArabic,
                Iso2Code = x.Iso2Code,
                Iso3Code = x.Iso3Code,
                PhoneCode = x.PhoneCode,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
