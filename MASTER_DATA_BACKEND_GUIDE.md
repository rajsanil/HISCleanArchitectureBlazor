# Backend Implementation Guide for Master Data CRUD

This guide explains what needs to be implemented in the Application layer to enable full CRUD functionality for the master data pages.

## Overview

Five dialog components have been created with commented-out code ready for backend integration:
- BloodGroupFormDialog.razor
- NationalityFormDialog.razor  
- MaritalStatusFormDialog.razor
- CountryFormDialog.razor
- CityFormDialog.razor

## Required Backend Implementation Per Entity

For each entity (BloodGroup, Nationality, MaritalStatus, Country, City), you need to create:

### 1. Commands Folder Structure
```
Application/Features/{Entity}/
├── Commands/
│   ├── AddEdit/
│   │   ├── AddEdit{Entity}Command.cs
│   │   ├── AddEdit{Entity}CommandValidator.cs
│   │   └── AddEdit{Entity}CommandHandler.cs
│   └── Delete/
│       ├── Delete{Entity}Command.cs
│       └── Delete{Entity}CommandHandler.cs
```

### 2. AddEdit Command Example (BloodGroup)

**AddEdit{Entity}Command.cs**:
```csharp
namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.Commands.AddEdit;

public class AddEditBloodGroupCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    
    public string CacheKey => BloodGroupCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BloodGroupCacheKey.Tags;
}
```

**AddEdit{Entity}CommandValidator.cs**:
```csharp
public class AddEditBloodGroupCommandValidator : AbstractValidator<AddEditBloodGroupCommand>
{
    public AddEditBloodGroupCommandValidator()
    {
        RuleFor(v => v.Code)
            .MaximumLength(50)
            .NotEmpty();
            
        RuleFor(v => v.Name)
            .MaximumLength(256)
            .NotEmpty();
            
        RuleFor(v => v.NameArabic)
            .MaximumLength(256);
    }
}
```

**AddEdit{Entity}CommandHandler.cs**:
```csharp
public class AddEditBloodGroupCommandHandler : IRequestHandler<AddEditBloodGroupCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public AddEditBloodGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditBloodGroupCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            // Update existing
            var item = await _context.BloodGroups.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
                return await Result<int>.FailureAsync($"Blood Group not found.");
            
            item.Code = request.Code;
            item.Name = request.Name;
            item.NameArabic = request.NameArabic;
            item.DisplayOrder = request.DisplayOrder;
            item.IsActive = request.IsActive;
            
            item.AddDomainEvent(new UpdatedEvent<BloodGroup>(item));
            await _context.SaveChangesAsync(cancellationToken);
            
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            // Create new
            var item = new BloodGroup
            {
                Code = request.Code,
                Name = request.Name,
                NameArabic = request.NameArabic,
                DisplayOrder = request.DisplayOrder,
                IsActive = request.IsActive
            };
            
            item.AddDomainEvent(new CreatedEvent<BloodGroup>(item));
            _context.BloodGroups.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            
            return await Result<int>.SuccessAsync(item.Id);
        }
    }
}
```

### 3. Delete Command Example

**Delete{Entity}Command.cs**:
```csharp
public record DeleteBloodGroupCommand(int[] Ids) : ICacheInvalidatorRequest<Result<int>>
{
    public string CacheKey => BloodGroupCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => BloodGroupCacheKey.Tags;
}
```

**Delete{Entity}CommandHandler.cs**:
```csharp
public class DeleteBloodGroupCommandHandler : IRequestHandler<DeleteBloodGroupCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public DeleteBloodGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(DeleteBloodGroupCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.BloodGroups
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
            
        if (!items.Any())
            return await Result<int>.FailureAsync("No blood groups found for deletion.");
            
        foreach (var item in items)
        {
            item.AddDomainEvent(new DeletedEvent<BloodGroup>(item));
            _context.BloodGroups.Remove(item);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(items.Count);
    }
}
```

### 4. Cache Keys

Create cache key class if it doesn't exist:

```csharp
// Application/Features/BloodGroups/Caching/BloodGroupCacheKey.cs
public static class BloodGroupCacheKey
{
    public const string GetAllCacheKey = "all-bloodgroups";
    public static readonly string[] Tags = { "bloodgroups" };
}
```

### 5. Permissions

Add to Infrastructure/PermissionSet/Permissions.cs:

```csharp
public static class BloodGroups
{
    public const string View = "Permissions.BloodGroups.View";
    public const string Create = "Permissions.BloodGroups.Create";
    public const string Edit = "Permissions.BloodGroups.Edit";
    public const string Delete = "Permissions.BloodGroups.Delete";
}
```

## Entity-Specific Notes

### Nationalities
- NO DisplayOrder field (DTO doesn't have it)
- Fields: Code, Name, NameArabic, IsActive

### Countries  
- Additional fields: Iso2Code, Iso3Code, PhoneCode
- NO DisplayOrder field
- Fields: Code, Name, NameArabic, Iso2Code, Iso3Code, PhoneCode, IsActive

### Cities
- Has foreign key: CountryId
- NO DisplayOrder field
- Requires Country selection in form
- Fields: Code, Name, NameArabic, CountryId, IsActive

### Blood Groups & Marital Statuses
- Have DisplayOrder field
- Fields: Code, Name, NameArabic, DisplayOrder, IsActive

## Steps to Enable Dialogs

For each entity:

1. ✅ Create Commands folder structure
2. ✅ Implement AddEditCommand with validator and handler
3. ✅ Implement DeleteCommand with handler
4. ✅ Create/update CacheKey class
5. ✅ Add permissions to Permissions.cs
6. ✅ Add permission seeds to database seeder
7. ✅ Uncomment dialog code in FormDialog.razor files
8. ✅ Wire up dialog in grid page (see next section)

## Wiring Up Dialogs in Grid Pages

Once backend is implemented, update each grid page:

**Add using statements**:
```csharp
@using CleanArchitecture.Blazor.Application.Features.BloodGroups.Commands.AddEdit
@using CleanArchitecture.Blazor.Application.Features.BloodGroups.Commands.Delete
```

**Add authorization attribute**:
```csharp
@attribute [Authorize(Policy = Permissions.BloodGroups.View)]
```

**Add permission fields**:
```csharp
private bool _canCreate;
private bool _canEdit;
private bool _canDelete;
```

**Initialize permissions**:
```csharp
protected override async Task OnInitializedAsync()
{
    var state = await AuthState;
    _canCreate = (await AuthService.AuthorizeAsync(state.User, Permissions.BloodGroups.Create)).Succeeded;
    _canEdit = (await AuthService.AuthorizeAsync(state.User, Permissions.BloodGroups.Edit)).Succeeded;
    _canDelete = (await AuthService.AuthorizeAsync(state.User, Permissions.BloodGroups.Delete)).Succeeded;
    
    await LoadData();
}
```

**Add toolbar button**:
```razor
@if (_canCreate)
{
    <MudButton StartIcon="@Icons.Material.Filled.Add"
               Variant="Variant.Filled"
               Color="Color.Primary"
               OnClick="OnCreate">
        @ConstantString.New
    </MudButton>
}
```

**Add action column**:
```razor
@if (_canEdit || _canDelete)
{
    <GridColumn Width="120px" Title="@L["Actions"]">
        <Template>
            @{
                var item = context as BloodGroupDto;
            }
            @if (_canEdit)
            {
                <MudIconButton Icon="@Icons.Material.Filled.Edit" 
                               Size="Size.Small" 
                               OnClick="@(() => OnEdit(item))" />
            }
            @if (_canDelete)
            {
                <MudIconButton Icon="@Icons.Material.Filled.Delete" 
                               Size="Size.Small" 
                               Color="Color.Error"
                               OnClick="@(() => OnDelete(item))" />
            }
        </Template>
    </GridColumn>
}
```

**Add CRUD methods**:
```csharp
private async Task OnCreate()
{
    var command = new AddEditBloodGroupCommand { IsActive = true };
    var parameters = new DialogParameters<BloodGroupFormDialog>
    {
        { x => x.Model, command },
        { x => x.Refresh, LoadData }
    };
    var dialog = await DialogService.ShowAsync<BloodGroupFormDialog>("Create Blood Group", parameters);
    var result = await dialog.Result;
    if (!result.Canceled)
    {
        await LoadData();
    }
}

private async Task OnEdit(BloodGroupDto dto)
{
    var command = new AddEditBloodGroupCommand
    {
        Id = dto.Id,
        Code = dto.Code,
        Name = dto.Name,
        NameArabic = dto.NameArabic,
        DisplayOrder = dto.DisplayOrder,
        IsActive = dto.IsActive
    };
    var parameters = new DialogParameters<BloodGroupFormDialog>
    {
        { x => x.Model, command },
        { x => x.Refresh, LoadData }
    };
    var dialog = await DialogService.ShowAsync<BloodGroupFormDialog>("Edit Blood Group", parameters);
    var result = await dialog.Result;
    if (!result.Canceled)
    {
        await LoadData();
    }
}

private async Task OnDelete(BloodGroupDto dto)
{
    var command = new DeleteBloodGroupCommand(new[] { dto.Id });
    var parameters = new DialogParameters<DeleteConfirmation>
    {
        { x => x.Command, command },
        { x => x.ContentText, $"Are you sure you want to delete '{dto.Name}'?" }
    };
    var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete Confirmation", parameters);
    var result = await dialog.Result;
    if (!result.Canceled)
    {
        await LoadData();
    }
}
```

## Testing Checklist

After implementing backend for each entity:

- [ ] Create new record via dialog
- [ ] Edit existing record
- [ ] Delete record
- [ ] Verify validation errors show correctly
- [ ] Verify cache invalidation works
- [ ] Verify permissions restrict unauthorized users
- [ ] Test "Save and New" button
- [ ] Verify domain events are raised

## Current Status

✅ **UI Layer Complete**:
- All 5 master data grid pages created
- All 5 form dialogs created (code commented until backend ready)
- Menu navigation configured
- DeleteConfirmation dialog reused from shared components

⏳ **Backend Layer Pending**:
- Commands (AddEdit + Delete) for all 5 entities
- Command validators
- Command handlers
- Cache keys
- Permission definitions
- Permission seeds

Once backend is implemented, uncomment the dialog code and wire up the CRUD operations in the grid pages following the patterns above.
