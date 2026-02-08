using CleanArchitecture.Blazor.Application.Features.Nationalities.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Nationalities.Queries.GetAll;

public class GetAllNationalitiesQuery : IRequest<IEnumerable<NationalityDto>>
{
}

public class GetAllNationalitiesQueryHandler : IRequestHandler<GetAllNationalitiesQuery, IEnumerable<NationalityDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllNationalitiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NationalityDto>> Handle(GetAllNationalitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Nationalities
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new NationalityDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                NameArabic = x.NameArabic,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
