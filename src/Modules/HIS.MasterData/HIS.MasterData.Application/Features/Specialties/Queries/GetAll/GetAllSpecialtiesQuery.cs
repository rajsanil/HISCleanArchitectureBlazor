using HIS.MasterData.Application.Features.Specialties.Caching;
using HIS.MasterData.Application.Features.Specialties.DTOs;
using HIS.MasterData.Application.Features.Specialties.Mappers;

namespace HIS.MasterData.Application.Features.Specialties.Queries.GetAll;

public class GetAllSpecialtiesQuery : ICacheableRequest<IEnumerable<SpecialtyDto>>
{
    public string CacheKey => SpecialtyCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => SpecialtyCacheKey.Tags;
}

public class GetAllSpecialtiesQueryHandler : IRequestHandler<GetAllSpecialtiesQuery, IEnumerable<SpecialtyDto>>
{
    private readonly IMasterDataDbContext _context;
    public GetAllSpecialtiesQueryHandler(IMasterDataDbContext context) { _context = context; }

    public async Task<IEnumerable<SpecialtyDto>> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Specialties.ProjectTo().ToListAsync(cancellationToken);
    }
}
