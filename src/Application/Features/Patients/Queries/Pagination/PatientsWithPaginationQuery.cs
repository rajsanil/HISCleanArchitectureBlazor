using CleanArchitecture.Blazor.Application.Features.Patients.Caching;
using CleanArchitecture.Blazor.Application.Features.Patients.DTOs;
using CleanArchitecture.Blazor.Application.Features.Patients.Mappers;
using CleanArchitecture.Blazor.Application.Features.Patients.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Patients.Queries.Pagination;

public class PatientsWithPaginationQuery : PatientAdvancedFilter, ICacheableRequest<PaginatedData<PatientDto>>
{
    public PatientAdvancedSpecification Specification => new(this);
    public string CacheKey => PatientCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => PatientCacheKey.Tags;

    public override string ToString()
    {
        return
            $"CurrentUser:{CurrentUser?.UserId},ListView:{ListView},Search:{Keyword},MRN:{MRN},EmiratesId:{EmiratesId},Gender:{Gender},IsVIP:{IsVIP},IsActive:{IsActive},SortDirection:{SortDirection},OrderBy:{OrderBy},{PageNumber},{PageSize}";
    }
}

public class PatientsWithPaginationQueryHandler :
    IRequestHandler<PatientsWithPaginationQuery, PaginatedData<PatientDto>>
{
    private readonly IApplicationDbContext _context;

    public PatientsWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedData<PatientDto>> Handle(PatientsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _context.Patients
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync(request.Specification, request.PageNumber,
                request.PageSize, PatientMapper.ToDto, cancellationToken);
        return data;
    }
}
