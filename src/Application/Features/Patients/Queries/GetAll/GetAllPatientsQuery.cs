using CleanArchitecture.Blazor.Application.Features.Patients.Caching;
using CleanArchitecture.Blazor.Application.Features.Patients.DTOs;
using CleanArchitecture.Blazor.Application.Features.Patients.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Patients.Queries.GetAll;

public class GetAllPatientsQuery : ICacheableRequest<IEnumerable<PatientDto>>
{
    public string CacheKey => PatientCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => PatientCacheKey.Tags;
}

public class GetPatientQuery : ICacheableRequest<PatientDto?>
{
    public required int Id { get; set; }
    public string CacheKey => PatientCacheKey.GetPatientByIdCacheKey(Id);
    public IEnumerable<string>? Tags => PatientCacheKey.Tags;
}

public class GetAllPatientsQueryHandler :
    IRequestHandler<GetAllPatientsQuery, IEnumerable<PatientDto>>,
    IRequestHandler<GetPatientQuery, PatientDto?>
{
    private readonly IApplicationDbContext _context;

    public GetAllPatientsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Patients
            .ProjectTo()
            .ToListAsync(cancellationToken);
        return data;
    }

    public async Task<PatientDto?> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Patients
            .Where(x => x.Id == request.Id)
            .ProjectTo()
            .FirstOrDefaultAsync(cancellationToken);
        return data;
    }
}
