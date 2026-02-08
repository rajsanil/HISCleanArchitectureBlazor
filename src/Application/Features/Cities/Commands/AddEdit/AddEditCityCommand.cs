#nullable disable

using CleanArchitecture.Blazor.Application.Features.Cities.Caching;
using CleanArchitecture.Blazor.Application.Features.Cities.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Cities.Commands.AddEdit;

public class AddEditCityCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    
    [Description("Code")]
    public string Code { get; set; } = string.Empty;
    
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    
    [Description("Name (Arabic)")]
    public string? NameArabic { get; set; }
    
    [Description("Country")]
    public int? CountryId { get; set; }
    
    [Description("Is Active")]
    public bool IsActive { get; set; } = true;

    public string CacheKey => CityCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CityCacheKey.Tags;
}

public class AddEditCityCommandHandler : IRequestHandler<AddEditCityCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    
    public AddEditCityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<int>> Handle(AddEditCityCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Cities.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"City with id: [{request.Id}] not found.");
            }
            
            CityMapper.ApplyChangesFrom(request, item);
            item.AddDomainEvent(new CityUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = CityMapper.FromEditCommand(request);
            item.AddDomainEvent(new CityCreatedEvent(item));
            _context.Cities.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
