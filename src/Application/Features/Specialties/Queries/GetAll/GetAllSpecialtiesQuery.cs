using CleanArchitecture.Blazor.Application.Features.Specialties.Caching;
using CleanArchitecture.Blazor.Application.Features.Specialties.DTOs;
using CleanArchitecture.Blazor.Application.Features.Specialties.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Specialties.Queries.GetAll;

public class GetAllSpecialtiesQuery : ICacheableRequest<IEnumerable<SpecialtyDto>>
{
    public string CacheKey => SpecialtyCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => SpecialtyCacheKey.Tags;
}

public class GetAllSpecialtiesQueryHandler : IRequestHandler<GetAllSpecialtiesQuery, IEnumerable<SpecialtyDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAllSpecialtiesQueryHandler(IApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<SpecialtyDto>> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Specialties.ProjectTo().ToListAsync(cancellationToken);
    }
}
