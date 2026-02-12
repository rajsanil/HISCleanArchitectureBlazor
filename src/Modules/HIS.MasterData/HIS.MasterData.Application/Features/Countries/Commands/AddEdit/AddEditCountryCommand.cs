using HIS.MasterData.Application.Features.Countries.Caching;
using HIS.MasterData.Application.Features.Countries.Mappers;
using HIS.MasterData.Application.Common.Exceptions;

namespace HIS.MasterData.Application.Features.Countries.Commands.AddEdit;

public class AddEditCountryCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Code")]
    public string Code { get; set; } = string.Empty;

    [Description("Name")]
    public string Name { get; set; } = string.Empty;

    [Description("Name (Arabic)")]
    public string? NameArabic { get; set; }

    [Description("ISO2 Code")]
    public string? Iso2Code { get; set; }

    [Description("ISO3 Code")]
    public string? Iso3Code { get; set; }

    [Description("Phone Code")]
    public string? PhoneCode { get; set; }

    [Description("Is Active")]
    public bool IsActive { get; set; } = true;

    public string CacheKey => CountryCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CountryCacheKey.Tags;
}

public class AddEditCountryCommandHandler : IRequestHandler<AddEditCountryCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public AddEditCountryCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditCountryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                var item = await _context.Countries.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null) return await Result<int>.FailureAsync($"Country with id: [{request.Id}] not found.");
                CountryMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Country>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                var item = CountryMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Country>(item));
                _context.Countries.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
        }
        catch (DbUpdateException ex) when (UniqueConstraintExceptionHandler.IsUniqueConstraintViolation(ex))
        {
            // Clear the change tracker to remove any tracked entities with invalid state
            _context.ChangeTracker.Clear();
            
            var constraintName = UniqueConstraintExceptionHandler.ExtractConstraintName(ex);
            // Format: "FieldName|User-friendly message" — the UI parses the prefix to highlight the correct field
            return constraintName switch
            {
                "IX_Countries_Code" => await Result<int>.FailureAsync($"Code|A country with code '{request.Code}' already exists."),
                "IX_Countries_Name" => await Result<int>.FailureAsync($"Name|A country with name '{request.Name}' already exists."),
                "IX_Countries_Iso2Code" => await Result<int>.FailureAsync($"Iso2Code|A country with ISO2 code '{request.Iso2Code}' already exists."),
                "IX_Countries_Iso3Code" => await Result<int>.FailureAsync($"Iso3Code|A country with ISO3 code '{request.Iso3Code}' already exists."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
