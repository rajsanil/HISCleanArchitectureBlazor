# Master Data Entity — Create / Fix Prompt Template

> **Purpose**: Copy-paste this prompt into a new AI chat session to create a brand-new master data entity **or** fix an existing one. Fill in the `[PLACEHOLDERS]` and remove sections that don't apply.

---

## HOW TO USE

1. **New entity** → Fill in Section A + B, paste the whole prompt.
2. **Fix existing entity** → Fill in Section A, skip B, describe the bug in Section C.
3. **Add features to existing entity** → Fill in Section A, describe what's missing in Section C.

---

## PROMPT — START COPYING BELOW THIS LINE

---

### Context

I am working on a **.NET 10 Blazor Server** application using **Clean Architecture** with a modular HIS (Hospital Information System) structure. The project uses:

- **EF Core 10** (SQL Server) with soft-delete (`BaseAuditableSoftDeleteEntity`)
- **MediatR 12** for CQRS (Commands / Queries)
- **FluentValidation 12** for command validation
- **Riok.Mapperly** (source-generator mapper, `[Mapper]` attribute)
- **FusionCache** for query caching (`ICacheableRequest`, `ICacheInvalidatorRequest`)
- **MudBlazor 8** for forms/buttons + **Telerik UI for Blazor 12** for data grids
- **Pipe-delimited field-error pattern**: command returns `"FieldName|message"`, UI parses it to highlight the correct `MudTextField`

### Module Structure

```
src/Modules/HIS.MasterData/
├── HIS.MasterData.Domain/Entities/          ← Domain entities
├── HIS.MasterData.Infrastructure/
│   ├── Configurations/                      ← EF Fluent API configs
│   ├── Permissions/MasterDataPermissions.cs ← Permission constants
│   └── MasterDataModule.cs                  ← Module registration
├── HIS.MasterData.Application/
│   ├── Common/
│   │   ├── Interfaces/IMasterDataDbContext.cs  ← Module DbContext interface
│   │   └── Exceptions/UniqueConstraintExceptionHandler.cs
│   └── Features/{EntityPlural}/
│       ├── Caching/{Entity}CacheKey.cs
│       ├── DTOs/{Entity}Dto.cs
│       ├── Mappers/{Entity}Mapper.cs
│       ├── Commands/AddEdit/AddEdit{Entity}Command.cs
│       ├── Commands/AddEdit/AddEdit{Entity}CommandValidator.cs
│       ├── Commands/Delete/Delete{Entity}Command.cs
│       └── Queries/GetAll/GetAll{EntityPlural}Query.cs
└── HIS.MasterData.UI/
    ├── _Imports.razor                       ← Global usings (MudBlazor, Telerik, MediatR, etc.)
    └── Pages/{EntityPlural}/{EntityPlural}.razor
```

### Infrastructure Registration Points (touch each when creating a new entity)

| # | File | What to add |
|---|------|-------------|
| 1 | `IMasterDataDbContext.cs` | `DbSet<{Entity}> {EntityPlural} { get; set; }` |
| 2 | `ApplicationDbContext.cs` | Explicit interface: `DbSet<HIS.MasterData.Domain.Entities.{Entity}> IMasterDataDbContext.{EntityPlural} { get => Set<...>(); set { } }` |
| 3 | `MasterDataPermissions.cs` | New permission class with View/Create/Edit/Delete/Search/Export/Import |
| 4 | `MasterDataModule.cs` → `GetMenuSections()` | New `ModuleMenuItem` entry |

---

## SECTION A — Entity Definition

**Entity Name (singular)**: `[e.g. Country]`
**Entity Name (plural)**: `[e.g. Countries]`
**Table Name**: `[e.g. Countries]` (must match `builder.ToTable("...")`)
**Base Class**: `BaseAuditableSoftDeleteEntity` (gives Id, Created, CreatedBy, LastModified, LastModifiedBy, Deleted)

### Properties

| Property | Type | MaxLength | Required | Unique Index | Default | Notes |
|----------|------|-----------|----------|-------------|---------|-------|
| Code | string | 10 | Yes | IX_{TableName}_Code | `string.Empty` | Filtered: `[Deleted] IS NULL` |
| Name | string | 200 | Yes | IX_{TableName}_Name | `string.Empty` | Filtered: `[Deleted] IS NULL` |
| NameArabic | string? | 200 | No | — | `null` | |
| DisplayOrder | int | — | No | — | `0` | *(include only if entity needs ordering)* |
| IsActive | bool | — | No | — | `true` | `HasDefaultValue(true)` |
| *[FK field]* | int? | — | Yes/No | IX_{TableName}_{FK}_{Field} | `null` | *Composite unique with FK* |

> **Unique indexes with FK (composite)**: For entities like City that are unique *per parent*, define composite indexes: `IX_Cities_Code_CountryId` with filter `[Deleted] IS NULL`.

### Navigation Properties (if any)

| Property | Type | Relationship | Notes |
|----------|------|-------------|-------|
| *[e.g. Country]* | Country | Many-to-One | `public virtual Country? Country { get; set; }` |
| *[e.g. Cities]* | ICollection\<City\> | One-to-Many | On parent entity |

---

## SECTION B — What to Create (New Entity)

Check all files that need to be created:

- [ ] **Domain Entity** — `HIS.MasterData.Domain/Entities/{Entity}.cs`
- [ ] **EF Configuration** — `HIS.MasterData.Infrastructure/Configurations/{Entity}Configuration.cs`
- [ ] **IMasterDataDbContext** — Add DbSet line
- [ ] **ApplicationDbContext** — Add explicit interface implementation
- [ ] **Permissions** — Add to `MasterDataPermissions.cs`
- [ ] **Menu** — Add to `MasterDataModule.cs` → `GetMenuSections()`
- [ ] **CacheKey** — `Features/{EntityPlural}/Caching/{Entity}CacheKey.cs`
- [ ] **DTO** — `Features/{EntityPlural}/DTOs/{Entity}Dto.cs`
- [ ] **Mapper** — `Features/{EntityPlural}/Mappers/{Entity}Mapper.cs`
- [ ] **AddEdit Command** — `Features/{EntityPlural}/Commands/AddEdit/AddEdit{Entity}Command.cs`
- [ ] **AddEdit Validator** — `Features/{EntityPlural}/Commands/AddEdit/AddEdit{Entity}CommandValidator.cs`
- [ ] **Delete Command** — `Features/{EntityPlural}/Commands/Delete/Delete{Entity}Command.cs`
- [ ] **GetAll Query** — `Features/{EntityPlural}/Queries/GetAll/GetAll{EntityPlural}Query.cs`
- [ ] **Blazor Page** — `HIS.MasterData.UI/Pages/{EntityPlural}/{EntityPlural}.razor`

---

## SECTION C — What to Fix (Existing Entity)

**Entity being fixed**: `[e.g. BloodGroup]`
**Issue**: `[Describe the error — e.g. "Navigation shows Not Found", "Unique constraint not caught", "Build error CS0535", etc.]`
**Error message (if any)**: `[Paste exact error]`

---

## REFERENCE IMPLEMENTATION — Country (Gold Standard)

Below are the **exact file contents** of the Country master, which is the canonical pattern every other master must follow.

### 1. Domain Entity — `HIS.MasterData.Domain/Entities/Country.cs`

```csharp
using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace HIS.MasterData.Domain.Entities;

public class Country : BaseAuditableSoftDeleteEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public string? Iso2Code { get; set; }
    public string? Iso3Code { get; set; }
    public string? PhoneCode { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
}
```

### 2. EF Configuration — `HIS.MasterData.Infrastructure/Configurations/CountryConfiguration.cs`

```csharp
using HIS.MasterData.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HIS.MasterData.Infrastructure.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameArabic).HasMaxLength(200);
        builder.Property(x => x.Iso2Code).HasMaxLength(2);
        builder.Property(x => x.Iso3Code).HasMaxLength(3);
        builder.Property(x => x.PhoneCode).HasMaxLength(10);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.Iso2Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL AND [Iso2Code] IS NOT NULL");

        builder.HasIndex(x => x.Iso3Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL AND [Iso3Code] IS NOT NULL");
    }
}
```

**Key rules for EF Config**:
- Always `builder.ToTable("{PluralTableName}")`
- Always `builder.Ignore(e => e.DomainEvents)`
- Unique indexes **must** filter `[Deleted] IS NULL` (soft-delete compatible)
- Nullable unique columns add `AND [{Column}] IS NOT NULL` to filter
- Composite indexes: `builder.HasIndex(x => new { x.Code, x.CountryId }).IsUnique().HasFilter(...)`

### 3. IMasterDataDbContext — Add one line

```csharp
DbSet<Country> Countries { get; set; }
```

### 4. ApplicationDbContext — Explicit interface implementation

```csharp
DbSet<HIS.MasterData.Domain.Entities.Country> IMasterDataDbContext.Countries { get => Set<HIS.MasterData.Domain.Entities.Country>(); set { } }
```

> **IMPORTANT**: Must use explicit interface implementation with `{ get => Set<T>(); set { } }` pattern. This avoids ambiguity with existing public DbSets. A getter-only `=> Set<T>()` will cause `CS0535` (does not implement set).

### 5. Permissions — `MasterDataPermissions.cs`

```csharp
[DisplayName("Country Permissions")]
[Description("Set permissions for country management operations.")]
public static class Countries
{
    [Description("Allows viewing country details.")]
    public const string View = "Permissions.Countries.View";
    public const string Create = "Permissions.Countries.Create";
    public const string Edit = "Permissions.Countries.Edit";
    public const string Delete = "Permissions.Countries.Delete";
    public const string Search = "Permissions.Countries.Search";
    public const string Export = "Permissions.Countries.Export";
    public const string Import = "Permissions.Countries.Import";
}
```

### 6. Module Menu — `MasterDataModule.cs` → `GetMenuSections()`

```csharp
new ModuleMenuItem { Title = "Countries", Icon = "mdi-earth", Href = "/pages/countries", Roles = ["Admin", "Users"] },
```

### 7. CacheKey — `Features/Countries/Caching/CountryCacheKey.cs`

```csharp
namespace HIS.MasterData.Application.Features.Countries.Caching;

public static class CountryCacheKey
{
    public const string GetAllCacheKey = "all-Countries";
    public static string GetCountryByIdCacheKey(int id) => $"GetCountryById,{id}";
    public static IEnumerable<string>? Tags => new[] { "country" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
```

### 8. DTO — `Features/Countries/DTOs/CountryDto.cs`

```csharp
namespace HIS.MasterData.Application.Features.Countries.DTOs;

[Description("Countries")]
public class CountryDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("ISO2 Code")] public string? Iso2Code { get; set; }
    [Description("ISO3 Code")] public string? Iso3Code { get; set; }
    [Description("Phone Code")] public string? PhoneCode { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
```

> **DTO with FK display name** (e.g. City showing CountryName):
> ```csharp
> [Description("Country")] public string? CountryName { get; set; }
> ```
> The Mapper must use `[MapProperty]` to map `Country.Name` → `CountryName`.

### 9. Mapper — `Features/Countries/Mappers/CountryMapper.cs`

```csharp
using HIS.MasterData.Application.Features.Countries.Commands.AddEdit;
using HIS.MasterData.Application.Features.Countries.DTOs;

namespace HIS.MasterData.Application.Features.Countries.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class CountryMapper
{
    public static partial CountryDto ToDto(Country country);
    public static partial Country FromDto(CountryDto dto);
    public static partial Country FromEditCommand(AddEditCountryCommand command);
    public static partial void ApplyChangesFrom(AddEditCountryCommand command, Country country);
    [MapperIgnoreSource(nameof(CountryDto.Id))]
    public static partial AddEditCountryCommand CloneFromDto(CountryDto dto);
    public static partial AddEditCountryCommand ToEditCommand(CountryDto dto);
    public static partial IQueryable<CountryDto> ProjectTo(this IQueryable<Country> q);
}
```

**Mapper rules for FK entities** (e.g. City with CountryName):
```csharp
// On ProjectTo — map navigation property name:
[MapProperty(new[] { nameof(City.Country), nameof(Country.Name) }, new[] { nameof(CityDto.CountryName) })]
public static partial IQueryable<CityDto> ProjectTo(this IQueryable<City> q);

// On CloneFromDto / ToEditCommand — ignore the display-only property:
[MapperIgnoreSource(nameof(CityDto.CountryName))]
[MapperIgnoreSource(nameof(CityDto.Id))]
public static partial AddEditCityCommand CloneFromDto(CityDto dto);
```

### 10. AddEdit Command — `Features/Countries/Commands/AddEdit/AddEditCountryCommand.cs`

```csharp
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
            var constraintName = UniqueConstraintExceptionHandler.ExtractConstraintName(ex);
            // Format: "FieldName|User-friendly message" — UI parses prefix to highlight correct field
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
```

**Key pattern**: The `catch` block uses `UniqueConstraintExceptionHandler` to extract the index name, then returns `"FieldName|message"` so the UI can highlight the correct field. The switch must list every unique index defined in the EF configuration.

### 11. Validator — `Features/Countries/Commands/AddEdit/AddEditCountryCommandValidator.cs`

```csharp
namespace HIS.MasterData.Application.Features.Countries.Commands.AddEdit;

public class AddEditCountryCommandValidator : AbstractValidator<AddEditCountryCommand>
{
    public AddEditCountryCommandValidator()
    {
        RuleFor(v => v.Code).MaximumLength(10).NotEmpty();
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.NameArabic).MaximumLength(200);
        RuleFor(v => v.Iso2Code).MaximumLength(2);
        RuleFor(v => v.Iso3Code).MaximumLength(3);
        RuleFor(v => v.PhoneCode).MaximumLength(10);
    }
}
```

> MaxLength values must match the EF configuration exactly. Required fields get `.NotEmpty()`. FK fields that are required get `.NotNull()`.

### 12. Delete Command — `Features/Countries/Commands/Delete/DeleteCountryCommand.cs`

```csharp
using HIS.MasterData.Application.Features.Countries.Caching;

namespace HIS.MasterData.Application.Features.Countries.Commands.Delete;

public class DeleteCountryCommand : ICacheInvalidatorRequest<Result<int>>
{
    public DeleteCountryCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string CacheKey => CountryCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CountryCacheKey.Tags;
}

public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public DeleteCountryCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Countries
            .Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<Country>(item));
            _context.Countries.Remove(item);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }
}
```

### 13. GetAll Query — `Features/Countries/Queries/GetAll/GetAllCountriesQuery.cs`

```csharp
using HIS.MasterData.Application.Features.Countries.Caching;
using HIS.MasterData.Application.Features.Countries.DTOs;
using HIS.MasterData.Application.Features.Countries.Mappers;

namespace HIS.MasterData.Application.Features.Countries.Queries.GetAll;

public class GetAllCountriesQuery : ICacheableRequest<IEnumerable<CountryDto>>
{
    public string CacheKey => CountryCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CountryCacheKey.Tags;
}

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
{
    private readonly IMasterDataDbContext _context;

    public GetAllCountriesQueryHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries.ProjectTo().ToListAsync(cancellationToken);
    }
}
```

> For entities with `DisplayOrder`, add `.OrderBy(x => x.DisplayOrder)` before `.ProjectTo()`.
> For entities without `DisplayOrder`, add `.OrderBy(x => x.Name)` before `.ProjectTo()`.

### 14. UniqueConstraintExceptionHandler — Shared utility

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace HIS.MasterData.Application.Common.Exceptions;

public static class UniqueConstraintExceptionHandler
{
    public static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        var sqlException = ex.InnerException as SqlException;
        if (sqlException != null)
        {
            return sqlException.Number == 2601 || sqlException.Number == 2627;
        }
        var message = ex.InnerException?.Message ?? ex.Message;
        return message.Contains("unique", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("constraint", StringComparison.OrdinalIgnoreCase);
    }

    public static string? ExtractConstraintName(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message ?? ex.Message;
        var match = System.Text.RegularExpressions.Regex.Match(message,
            @"(?:unique\s+)?index\s+'([^']+)'",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (match.Success) return match.Groups[1].Value;

        match = System.Text.RegularExpressions.Regex.Match(message,
            @"constraint\s+'([^']+)'",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (match.Success) return match.Groups[1].Value;

        match = System.Text.RegularExpressions.Regex.Match(message,
            @"constraint\s+""([^""]+)""",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (match.Success) return match.Groups[1].Value;

        return null;
    }

    public static string GetUserFriendlyMessage(string? constraintName,
        string defaultMessage = "A record with the same value already exists.")
    {
        if (string.IsNullOrEmpty(constraintName)) return defaultMessage;
        var parts = constraintName.Split('_', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 3)
        {
            var fieldNames = parts.Skip(2).Select(FormatFieldName);
            return $"A record with the same {string.Join(" and ", fieldNames)} already exists.";
        }
        return defaultMessage;
    }

    private static string FormatFieldName(string fieldName)
    {
        return System.Text.RegularExpressions.Regex.Replace(fieldName, "([a-z])([A-Z])", "$1 $2").ToLowerInvariant();
    }
}
```

### 15. Blazor UI Page — `HIS.MasterData.UI/Pages/Countries/Countries.razor`

```razor
@page "/pages/countries"
@using HIS.MasterData.Application.Features.Countries.DTOs
@using HIS.MasterData.Application.Features.Countries.Mappers
@using HIS.MasterData.Application.Features.Countries.Queries.GetAll
@using HIS.MasterData.Application.Features.Countries.Caching
@using HIS.MasterData.Application.Features.Countries.Commands.AddEdit
@using HIS.MasterData.Application.Features.Countries.Commands.Delete

@attribute [Authorize(Policy = MasterDataPermissions.Countries.View)]
@inject IStringLocalizer<Countries> L

<PageTitle>@Title</PageTitle>

<MudPaper Elevation="0" Class="pa-4">
    @* -- Header Toolbar -- *@
    <MudStack Row Spacing="0" Class="mb-3" Justify="Justify.SpaceBetween">
        <MudStack Row AlignItems="AlignItems.Center">
            <MudIcon Icon="@Icons.Material.Filled.Public" Size="Size.Large" Color="Color.Primary" />
            <MudStack Spacing="0">
                <MudText Typo="Typo.h6">@Title</MudText>
                <MudText Typo="Typo.body2" Color="Color.Secondary">@L["Manage countries and regions"]</MudText>
            </MudStack>
        </MudStack>
        <MudStack Row Spacing="1" AlignItems="AlignItems.Center" Class="flex-wrap">
            @if (_canSearch)
            {
                <MudTextField T="string"
                              Value="@_searchString"
                              ValueChanged="@OnSearch"
                              Placeholder="@ConstantString.Search"
                              Adornment="Adornment.Start"
                              AdornmentIcon="@Icons.Material.Filled.Search"
                              IconSize="Size.Small"
                              Class="mt-0"
                              Style="min-width:200px;" />
            }
            <MudButton Variant="Variant.Outlined" Color="Color.Default"
                       StartIcon="@Icons.Material.Outlined.Refresh"
                       OnClick="OnRefresh" Disabled="@_loading">
                @ConstantString.Refresh
            </MudButton>
            @if (_canCreate)
            {
                <MudButton Variant="Variant.Filled" Color="Color.Primary"
                           StartIcon="@Icons.Material.Outlined.Add" OnClick="OnCreate">
                    @ConstantString.New
                </MudButton>
                <MudButton Variant="Variant.Outlined" Color="Color.Primary"
                           StartIcon="@Icons.Material.Outlined.ContentCopy"
                           OnClick="OnClone" Disabled="@(_selectedItems?.Count() != 1)">
                    @ConstantString.Clone
                </MudButton>
            }
            @if (_canDelete)
            {
                <MudButton Variant="Variant.Outlined" Color="Color.Error"
                           StartIcon="@Icons.Material.Outlined.Delete"
                           OnClick="OnDeleteChecked"
                           Disabled="@(!(_selectedItems?.Any() ?? false))">
                    @ConstantString.Delete (@(_selectedItems?.Count() ?? 0))
                </MudButton>
            }
        </MudStack>
    </MudStack>

    @* -- Telerik Data Grid -- *@
    <div class="telerik-grid-container">
        <TelerikGrid Data="@_filteredItems"
                     Pageable="true" PageSize="@_pageSize" @bind-Page="@_currentPage"
                     Sortable="true" SortMode="@Telerik.Blazor.SortMode.Multiple"
                     FilterMode="@GridFilterMode.FilterRow"
                     Resizable="true" Reorderable="true"
                     SelectionMode="@GridSelectionMode.Multiple"
                     @bind-SelectedItems="@_selectedItems"
                     Height="calc(100vh - 340px)"
                     OnRowDoubleClick="@OnRowDoubleClick">
            <GridColumns>
                <GridCheckboxColumn Width="50px" SelectAll="true" />
                @* Add one GridColumn per DTO property *@
                <GridColumn Field="@nameof(CountryDto.Code)" Title="@L["Code"]" Width="120px" />
                <GridColumn Field="@nameof(CountryDto.Name)" Title="@L["Name"]" Width="220px" />
                <GridColumn Field="@nameof(CountryDto.NameArabic)" Title="@L["Name (Arabic)"]" Width="200px" />
                <GridColumn Field="@nameof(CountryDto.IsActive)" Title="@L["Active"]" Width="110px"
                            TextAlign="@ColumnTextAlign.Center">
                    <Template>
                        @{ var item = (CountryDto)context; }
                        <MudChip T="string" Size="Size.Small"
                                 Color="@(item.IsActive ? Color.Success : Color.Default)"
                                 Variant="Variant.Filled">
                            @(item.IsActive ? L["Active"] : L["Inactive"])
                        </MudChip>
                    </Template>
                </GridColumn>
                <GridColumn Title="@ConstantString.Actions" Width="120px"
                            Filterable="false" Sortable="false" TextAlign="@ColumnTextAlign.Center">
                    <Template>
                        @{ var item = (CountryDto)context; }
                        @if (_canEdit)
                        {
                            <MudIconButton Icon="@Icons.Material.Outlined.Edit" Size="Size.Small"
                                           Color="Color.Primary" OnClick="@(() => OnEdit(item))" />
                        }
                        @if (_canDelete)
                        {
                            <MudIconButton Icon="@Icons.Material.Outlined.Delete" Size="Size.Small"
                                           Color="Color.Error" OnClick="@(() => OnDelete(item))" />
                        }
                    </Template>
                </GridColumn>
            </GridColumns>
        </TelerikGrid>
    </div>
</MudPaper>

@* -- TelerikWindow Form -- *@
<TelerikWindow @bind-Visible="@_formVisible" Modal="true" Width="780px">
    <WindowTitle>
        <MudStack Row AlignItems="AlignItems.Center" Spacing="2">
            <MudIcon Icon="@Icons.Material.Filled.Public" Color="Color.Primary" />
            <MudText Typo="Typo.h6">@_formTitle</MudText>
        </MudStack>
    </WindowTitle>
    <WindowActions><WindowAction Name="Close" /></WindowActions>
    <WindowContent>
        @if (_editModel is not null)
        {
            <MudForm @ref="_form" Model="@_editModel"
                     Validation="@(Validator.ValidateValue(_editModel))" ValidationDelay="0">
                <MudGrid Spacing="3" Class="pa-2">
                    @* Fields with unique constraints get @ref, Error, ErrorText bindings *@
                    <MudItem xs="12" sm="4">
                        <MudTextField @ref="_codeField" Label="@L["Code"]"
                                      @bind-Value="_editModel.Code"
                                      For="@(() => _editModel.Code)" Required="true"
                                      Variant="Variant.Outlined"
                                      Error="@_fieldErrors.ContainsKey("Code")"
                                      ErrorText="@GetFieldError("Code")" />
                    </MudItem>
                    <MudItem xs="12" sm="4">
                        <MudTextField @ref="_nameField" Label="@L["Name"]"
                                      @bind-Value="_editModel.Name"
                                      For="@(() => _editModel.Name)" Required="true"
                                      Variant="Variant.Outlined"
                                      Error="@_fieldErrors.ContainsKey("Name")"
                                      ErrorText="@GetFieldError("Name")" />
                    </MudItem>
                    @* Regular fields — no error binding needed *@
                    <MudItem xs="12" sm="4">
                        <MudTextField Label="@L["Name (Arabic)"]"
                                      @bind-Value="_editModel.NameArabic"
                                      For="@(() => _editModel.NameArabic)"
                                      Variant="Variant.Outlined" />
                    </MudItem>
                    @* FK dropdown example (for City → Country):
                    <MudItem xs="12" sm="6">
                        <MudSelect T="int?" Label="@L["Country"]"
                                   @bind-Value="_editModel.CountryId"
                                   Required="true" Variant="Variant.Outlined"
                                   AnchorOrigin="Origin.BottomCenter">
                            @foreach (var country in _countries)
                            {
                                <MudSelectItem T="int?" Value="@country.Id">@country.Name</MudSelectItem>
                            }
                        </MudSelect>
                    </MudItem>
                    *@
                    <MudItem xs="12" sm="4" Class="d-flex align-center">
                        <MudCheckBox Label="@L["Is Active"]"
                                     @bind-Value="_editModel.IsActive" Color="Color.Primary" />
                    </MudItem>
                </MudGrid>
            </MudForm>
            <MudStack Row Spacing="2" Justify="Justify.FlexEnd" Class="pa-2 mt-2">
                <MudButton Variant="Variant.Text" Color="Color.Default" OnClick="CloseForm">
                    @ConstantString.Cancel
                </MudButton>
                <MudLoadingButton Variant="Variant.Outlined" Color="Color.Primary"
                                  Loading="@_savingNew" OnClick="SaveAndNew">
                    @ConstantString.SaveAndNew
                </MudLoadingButton>
                <MudLoadingButton Variant="Variant.Filled" Color="Color.Primary"
                                  Loading="@_saving" OnClick="SubmitForm">
                    @ConstantString.Save
                </MudLoadingButton>
            </MudStack>
        }
    </WindowContent>
</TelerikWindow>

@code {
    [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;
    [CascadingParameter] private UserProfile? UserProfile { get; set; }

    public string? Title { get; private set; }
    private List<CountryDto> _allCountries = new();
    private List<CountryDto> _filteredItems = new();
    private IEnumerable<CountryDto> _selectedItems { get; set; } = new List<CountryDto>();
    private bool _loading;
    private int _pageSize = 20;
    private int _currentPage = 1;
    private string _searchString = string.Empty;

    private bool _canSearch, _canCreate, _canEdit, _canDelete;

    private bool _formVisible;
    private string _formTitle = string.Empty;
    private AddEditCountryCommand? _editModel;
    private MudForm? _form;
    private bool _saving, _savingNew;

    // -- Field-level error handling --
    private Dictionary<string, string> _fieldErrors = new();
    private MudTextField<string>? _codeField;   // one ref per unique-indexed field
    private MudTextField<string>? _nameField;

    protected override async Task OnInitializedAsync()
    {
        Title = L["Countries"];
        var state = await AuthState;
        _canCreate = (await AuthService.AuthorizeAsync(state.User, MasterDataPermissions.Countries.Create)).Succeeded;
        _canSearch = (await AuthService.AuthorizeAsync(state.User, MasterDataPermissions.Countries.Search)).Succeeded;
        _canEdit   = (await AuthService.AuthorizeAsync(state.User, MasterDataPermissions.Countries.Edit)).Succeeded;
        _canDelete = (await AuthService.AuthorizeAsync(state.User, MasterDataPermissions.Countries.Delete)).Succeeded;
        await LoadData();
    }

    private async Task LoadData()
    {
        _loading = true;
        try
        {
            var result = await Mediator.Send(new GetAllCountriesQuery());
            _allCountries = result.ToList();
            ApplySearch();
        }
        finally { _loading = false; }
    }

    private void ApplySearch()
    {
        _filteredItems = string.IsNullOrWhiteSpace(_searchString)
            ? _allCountries.ToList()
            : _allCountries.Where(x =>
                x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                x.Code.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                (x.NameArabic?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
    }

    private void OnSearch(string text) { _searchString = text; ApplySearch(); }

    private async Task OnRefresh()
    {
        CountryCacheKey.Refresh();
        _selectedItems = new List<CountryDto>();
        _searchString = string.Empty;
        await LoadData();
    }

    private void OnRowDoubleClick(GridRowClickEventArgs args)
    {
        if (_canEdit && args.Item is CountryDto dto) OnEdit(dto);
    }

    // -- CRUD --

    private void OnCreate()
    {
        ClearFieldErrors();
        _editModel = new AddEditCountryCommand();
        _formTitle = string.Format(ConstantString.CreateAnItem, L["Country"]);
        _formVisible = true;
    }

    private void OnClone()
    {
        var copy = _selectedItems.FirstOrDefault();
        if (copy is null) return;
        ClearFieldErrors();
        _editModel = CountryMapper.CloneFromDto(copy);
        _formTitle = string.Format(ConstantString.CreateAnItem, L["Country"]);
        _formVisible = true;
    }

    private void OnEdit(CountryDto dto)
    {
        ClearFieldErrors();
        _editModel = CountryMapper.ToEditCommand(dto);
        _formTitle = string.Format(ConstantString.EditTheItem, L["Country"]);
        _formVisible = true;
    }

    private async Task OnDelete(CountryDto dto)
    {
        var contentText = string.Format(ConstantString.DeleteConfirmation, dto.Name);
        var command = new DeleteCountryCommand(new[] { dto.Id });
        await DialogServiceHelper.ShowDeleteConfirmationDialogAsync(
            command, ConstantString.DeleteConfirmationTitle, contentText, async () =>
            {
                await InvokeAsync(async () => { await LoadData(); _selectedItems = new List<CountryDto>(); });
            });
    }

    private async Task OnDeleteChecked()
    {
        var selected = _selectedItems.ToList();
        if (!selected.Any()) return;
        var contentText = string.Format(ConstantString.DeleteConfirmWithSelected, selected.Count);
        var command = new DeleteCountryCommand(selected.Select(x => x.Id).ToArray());
        await DialogServiceHelper.ShowDeleteConfirmationDialogAsync(
            command, ConstantString.DeleteConfirmationTitle, contentText, async () =>
            {
                await InvokeAsync(async () => { await LoadData(); _selectedItems = new List<CountryDto>(); });
            });
    }

    // -- Field Error Helpers --

    private string? GetFieldError(string fieldName) =>
        _fieldErrors.TryGetValue(fieldName, out var error) ? error : null;

    private void ClearFieldErrors() => _fieldErrors.Clear();

    private static (string? FieldName, string Message) ParseFieldError(string errorMessage)
    {
        var pipeIndex = errorMessage.IndexOf('|');
        if (pipeIndex > 0 && pipeIndex < 20)
            return (errorMessage[..pipeIndex], errorMessage[(pipeIndex + 1)..]);
        return (null, errorMessage);
    }

    private void SetFieldError(string errorMessage)
    {
        ClearFieldErrors();
        var (fieldName, message) = ParseFieldError(errorMessage);
        if (fieldName is null) return;
        _fieldErrors[fieldName] = message;
        // Auto-focus the errored field
        _ = fieldName switch
        {
            "Code" => _codeField?.FocusAsync(),
            "Name" => _nameField?.FocusAsync(),
            _ => null
        };
    }

    // -- Save actions --

    private async Task SubmitForm()
    {
        if (_editModel is null) return;
        _saving = true;
        try
        {
            ClearFieldErrors();
            await _form!.Validate();
            if (!_form!.IsValid) return;
            var result = await Mediator.Send(_editModel);
            result.Match(
                data => { _formVisible = false; Snackbar.Add(ConstantString.SaveSuccess, Severity.Info); return data; },
                errors => { SetFieldError(errors); var (_, msg) = ParseFieldError(errors); Snackbar.Add(msg, Severity.Error); return 0; });
            if (!_formVisible) { await LoadData(); _selectedItems = new List<CountryDto>(); }
        }
        finally { _saving = false; }
    }

    private async Task SaveAndNew()
    {
        if (_editModel is null) return;
        _savingNew = true;
        try
        {
            ClearFieldErrors();
            await _form!.Validate();
            if (!_form!.IsValid) return;
            var result = await Mediator.Send(_editModel);
            var success = result.Match(
                data => { Snackbar.Add(ConstantString.SaveSuccess, Severity.Info); _editModel = new AddEditCountryCommand(); ClearFieldErrors(); return true; },
                errors => { SetFieldError(errors); var (_, msg) = ParseFieldError(errors); Snackbar.Add(msg, Severity.Error); return false; });
            if (success) await LoadData();
        }
        finally { _savingNew = false; }
    }

    private void CloseForm() { _formVisible = false; _editModel = null; ClearFieldErrors(); }
}
```

### 16. _Imports.razor — Global usings injected into every page

```razor
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Authorization
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Authorization
@using Microsoft.Extensions.Localization
@using MediatR
@using MudBlazor
@using Telerik.Blazor
@using Telerik.Blazor.Components
@using BlazorDownloadFile
@using System.Net.Http
@using CleanArchitecture.Blazor.Application.Common.Interfaces
@using CleanArchitecture.Blazor.Application.Common.Security
@using CleanArchitecture.Blazor.Application.Common.Models
@using CleanArchitecture.Blazor.Application.Common.Extensions
@using CleanArchitecture.Blazor.Domain.Entities
@using HIS.MasterData.Infrastructure.Permissions
@using HIS.Core.UI.Constants
@using HIS.Core.UI.Components.LoadingButton
@using HIS.Core.UI.Components.Forms
@using HIS.Core.UI.Components.Dialogs
@using HIS.Core.UI.Components.Autocompletes
@using HIS.Core.UI.Services
@inject IValidationService Validator
@inject ISender Mediator
@inject ISnackbar Snackbar
@inject IAuthorizationService AuthService
@inject NavigationManager Navigation
@inject IDialogService DialogService
@inject DialogServiceHelper DialogServiceHelper
```

> These are **already available** in all pages — do NOT duplicate them in page-level `@using` directives.

---

## CHECKLISTS

### Pre-Build Verification

Before building, verify:

- [ ] Entity namespace is `HIS.MasterData.Domain.Entities`
- [ ] EF Config has `builder.ToTable("{PluralTableName}")` and `builder.Ignore(e => e.DomainEvents)`
- [ ] Every unique index has `.HasFilter("[Deleted] IS NULL")`
- [ ] `IMasterDataDbContext` has `DbSet<{Entity}> {EntityPlural} { get; set; }`
- [ ] `ApplicationDbContext` has explicit interface: `DbSet<...> IMasterDataDbContext.{EntityPlural} { get => Set<...>(); set { } }`
- [ ] Command's `catch` block has a `case` for **every** unique index name
- [ ] Validator MaxLength values match EF Config
- [ ] Mapper has `#pragma warning disable RMG020` and `#pragma warning disable RMG012`
- [ ] Page route matches menu Href (e.g. `@page "/pages/countries"` matches `Href = "/pages/countries"`)
- [ ] Page `@attribute [Authorize(Policy = ...)]` uses the correct permission constant

### Common Errors & Fixes

| Error | Cause | Fix |
|-------|-------|-----|
| `CS0535: does not implement interface member '{Entity}Plural.set'` | Using `=> Set<T>()` (getter-only) instead of `{ get => Set<T>(); set { } }` | Use explicit interface: `IMasterDataDbContext.{EntityPlural} { get => Set<T>(); set { } }` |
| `SqlException: Invalid object name '{Entity}'` | Missing `builder.ToTable("{PluralTableName}")` in EF Config | Add `builder.ToTable(...)` |
| Page shows "Not Found" | Missing `@page` directive or page file doesn't exist | Create the .razor file with correct route |
| Unique constraint not caught | Missing `catch (DbUpdateException ex) when (...)` in command handler | Add the try/catch with `UniqueConstraintExceptionHandler` |
| Field error not highlighting | `_fieldErrors` key doesn't match the prefix in `"FieldName\|message"` | Ensure the switch case returns the correct field prefix |
| `RMG020` / `RMG012` warnings | Mapperly unmapped members | Add `#pragma warning disable RMG020` / `RMG012` |
| Mapper fails for FK display property | `CountryName` can't map back to command | Add `[MapperIgnoreSource(nameof({Dto}.CountryName))]` on `CloneFromDto`/`ToEditCommand` |
| Cache not invalidating | Missing `ICacheInvalidatorRequest` on command | Ensure command implements `ICacheInvalidatorRequest<Result<int>>` with CacheKey + Tags |

---

## PROMPT — END

Now please [create / fix] the **[EntityName]** master data entity following the patterns above exactly. Create all necessary files and register them in the infrastructure. Build and verify 0 errors.
