using CleanArchitecture.Blazor.Application.Features.Facilities.Caching;
using CleanArchitecture.Blazor.Application.Features.Facilities.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Facilities.Commands.AddEdit;

public class AddEditFacilityCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public string CacheKey => FacilityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => FacilityCacheKey.Tags;
}

public class AddEditFacilityCommandHandler : IRequestHandler<AddEditFacilityCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public AddEditFacilityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditFacilityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Facilities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null)
                    return await Result<int>.FailureAsync($"Facility with id: [{request.Id}] not found.");
                FacilityMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Facility>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = FacilityMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Facility>(item));
                _context.Facilities.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Facilities_Code") == true)
        {
            _context.ChangeTracker.Clear();
            return await Result<int>.FailureAsync($"Code|A facility with code '{request.Code}' already exists.");
        }
    }
}
