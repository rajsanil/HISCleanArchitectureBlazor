using HIS.MasterData.Application.Features.Cities.Caching;
using HIS.MasterData.Application.Features.Cities.Mappers;
using HIS.MasterData.Application.Common.Exceptions;

namespace HIS.MasterData.Application.Features.Cities.Commands.AddEdit;

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
    private readonly IMasterDataDbContext _context;

    public AddEditCityCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditCityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Cities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null) return await Result<int>.FailureAsync($"City with id: [{request.Id}] not found.");
                CityMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<City>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = CityMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<City>(item));
                _context.Cities.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
        }
        catch (DbUpdateException ex) when (UniqueConstraintExceptionHandler.IsUniqueConstraintViolation(ex))
        {
            var constraintName = UniqueConstraintExceptionHandler.ExtractConstraintName(ex);
            return constraintName switch
            {
                "IX_Cities_Code_CountryId" => await Result<int>.FailureAsync($"Code|A city with code '{request.Code}' already exists in this country."),
                "IX_Cities_Name_CountryId" => await Result<int>.FailureAsync($"Name|A city with name '{request.Name}' already exists in this country."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
