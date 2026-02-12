# Unique Constraint Exception Handling Pattern

## Overview
This document describes how to handle unique constraint violations at both the **Command Level** (Application Layer) and **UI Level** (Presentation Layer) in the Clean Architecture solution.

## Implementation

### 1. Command Level (Application Layer)

The command handler catches `DbUpdateException` and checks if it's a unique constraint violation, then returns user-friendly error messages.

#### Pattern:

```csharp
using HIS.MasterData.Application.Common.Exceptions;

public class AddEditEntityCommandHandler : IRequestHandler<AddEditEntityCommand, Result<int>>
{
    private readonly IMasterDataDbContext _context;

    public AddEditEntityCommandHandler(IMasterDataDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditEntityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id > 0)
            {
                // Update existing entity
                var item = await _context.Entities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (item == null) return await Result<int>.FailureAsync($"Entity not found.");
                
                // Apply changes
                EntityMapper.ApplyChangesFrom(request, item);
                item.AddDomainEvent(new UpdatedEvent<Entity>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
            else
            {
                // Create new entity
                var item = EntityMapper.FromEditCommand(request);
                item.AddDomainEvent(new CreatedEvent<Entity>(item));
                _context.Entities.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id);
            }
        }
        catch (DbUpdateException ex) when (UniqueConstraintExceptionHandler.IsUniqueConstraintViolation(ex))
        {
            var constraintName = UniqueConstraintExceptionHandler.ExtractConstraintName(ex);
            
            // Return specific error messages based on constraint name
            return constraintName switch
            {
                "IX_Entities_Code" => await Result<int>.FailureAsync($"An entity with code '{request.Code}' already exists."),
                "IX_Entities_Name" => await Result<int>.FailureAsync($"An entity with name '{request.Name}' already exists."),
                "IX_Entities_Name_ParentId" => await Result<int>.FailureAsync($"An entity with name '{request.Name}' already exists in this parent."),
                _ => await Result<int>.FailureAsync(UniqueConstraintExceptionHandler.GetUserFriendlyMessage(constraintName))
            };
        }
    }
}
```

### 2. UI Level (Presentation Layer)

The UI displays the error message from the command handler, **highlights the problematic field**, and **preserves form data** for correction.

#### Setup:

Add field error tracking and references to fields that can have unique violations:

```csharp
// --- Private Fields ---
private Dictionary<string, string> _fieldErrors = new();
private MudTextField<string>? _codeField;
private MudTextField<string>? _nameField;
// ... other fields as needed
```

#### Form Fields with Error Display:

```razor
<MudTextField @ref="_codeField" 
              Label="Code" 
              @bind-Value="_editModel.Code" 
              For="@(() => _editModel.Code)" 
              Required="true" 
              Variant="Variant.Outlined" 
              Error="@_fieldErrors.ContainsKey("Code")" 
              ErrorText="@GetFieldError("Code")" />

<MudTextField @ref="_nameField" 
              Label="Name" 
              @bind-Value="_editModel.Name" 
              For="@(() => _editModel.Name)" 
              Required="true" 
              Variant="Variant.Outlined" 
              Error="@_fieldErrors.ContainsKey("Name")" 
              ErrorText="@GetFieldError("Name")" />
```

#### Helper Methods:

```csharp
// -- Field Error Helpers --

private string? GetFieldError(string fieldName) => 
    _fieldErrors.TryGetValue(fieldName, out var error) ? error : null;

private void ClearFieldErrors()
{
    _fieldErrors.Clear();
}

private void SetFieldError(string errorMessage)
{
    ClearFieldErrors();
    
    // Parse error message to determine which field has the error
    if (errorMessage.Contains("code '", StringComparison.OrdinalIgnoreCase) && 
        errorMessage.Contains("entity with code", StringComparison.OrdinalIgnoreCase))
    {
        _fieldErrors["Code"] = errorMessage;
        _codeField?.FocusAsync();
    }
    else if (errorMessage.Contains("name '", StringComparison.OrdinalIgnoreCase) && 
             errorMessage.Contains("entity with name", StringComparison.OrdinalIgnoreCase))
    {
        _fieldErrors["Name"] = errorMessage;
        _nameField?.FocusAsync();
    }
    // Add more field mappings as needed
}
```

#### Submit Form Pattern:

```csharp
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
            data =>
            {
                _formVisible = false;
                Snackbar.Add(ConstantString.SaveSuccess, Severity.Info);
                return data;
            },
            errors =>
            {
                SetFieldError(errors);
                Snackbar.Add(errors, Severity.Error);
                return 0;
            });
        
        // Only reload if successful (form is closed)
        if (!_formVisible)
        {
            await LoadData();
            _selectedItems = new List<EntityDto>();
        }
    }
    finally
    {
        _saving = false;
    }
}
```

#### Save and New Pattern:

```csharp
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
            data =>
            {
                Snackbar.Add(ConstantString.SaveSuccess, Severity.Info);
                _editModel = new AddEditEntityCommand();
                ClearFieldErrors();
                return true;
            },
            errors =>
            {
                SetFieldError(errors);
                Snackbar.Add(errors, Severity.Error);
                return false; // Keep form data for correction
            });
        
        // Only reload if successful
        if (success)
        {
            await LoadData();
        }
    }
    finally
    {
        _savingNew = false;
    }
}
```

#### Close Form Pattern:

```csharp
private void CloseForm()
{
    _formVisible = false;
    _editModel = null;
    ClearFieldErrors();
}
```

## Helper Class: UniqueConstraintExceptionHandler

Located at: `HIS.MasterData.Application/Common/Exceptions/UniqueConstraintExceptionHandler.cs`

### Key Methods:

#### `IsUniqueConstraintViolation(DbUpdateException ex)`
Checks if the exception is a unique constraint violation.
- SQL Server: Checks error numbers 2601 and 2627
- Other databases: Checks exception message for keywords

#### `ExtractConstraintName(DbUpdateException ex)`
Extracts the constraint/index name from the exception message.
- Parses formats like: `index name 'IX_Countries_Code'`
- Returns the constraint name or null

#### `GetUserFriendlyMessage(string? constraintName)`
Generates a user-friendly error message from the constraint name.
- Example: `IX_Entities_EmailAddress` → "A record with the same email address already exists."

## Benefits

1. ✅ **Consistent Error Handling**: All entities handle unique constraint violations the same way
2. ✅ **User-Friendly Messages**: Clear, specific error messages instead of technical database errors
3. ✅ **Field-Level Highlighting**: Problematic fields are visually highlighted with error messages
4. ✅ **Form Data Preservation**: User input is preserved on validation errors for easy correction
5. ✅ **Auto-Focus**: Automatically focuses on the field with the error
6. ✅ **Centralized Logic**: Reusable helper class reduces code duplication
7. ✅ **Database Agnostic**: Works with SQL Server, PostgreSQL, SQLite
8. ✅ **Type-Safe**: No magic strings or error code checking in UI
9. ✅ **Clean Architecture**: Business logic stays in Application layer, UI just displays results

## Common Unique Indexes in MasterData

| Entity | Unique Indexes |
|--------|---------------|
| Country | Code, Name, Iso2Code, Iso3Code |
| City | Code+CountryId, Name+CountryId |
| BloodGroup | Code, Name |
| MaritalStatus | Code, Name |
| Nationality | Code, Name |
| Department | Code+FacilityId, Name+FacilityId |
| Location | Code+FacilityId, Name+FacilityId |
| Specialty | Code, Name |
| Bed | Code+RoomId |

## Example Error Messages

| Constraint | User-Friendly Message |
|------------|----------------------|
| IX_Countries_Code | "A country with code 'US' already exists." |
| IX_Countries_Name | "A country with name 'United States' already exists." |
| IX_Cities_Name_CountryId | "A city with name 'New York' already exists in this country." |
| IX_Departments_Code_FacilityId | "A department with code 'ER' already exists in this facility." |

## Testing

To test unique constraint handling with field highlighting:

### Test Case 1: Duplicate Code on Save
1. Create a country with code "US" and name "United States"
2. Try to create another country with code "US"
3. **Expected results**:
   - ❌ Error notification appears: "A country with code 'US' already exists."
   - ❌ Code field is highlighted in red with error text below
   - ✅ Form remains open with all data preserved
   - ✅ Cursor automatically focuses on Code field
   - ✅ User can change code and save again

### Test Case 2: Duplicate Name on SaveAndNew
1. Create a country with code "US" and name "United States"
2. Click "Save & New"
3. Enter code "USA" and name "United States"
4. Click "Save & New"
5. **Expected results**:
   - ❌ Error notification appears: "A country with name 'United States' already exists."
   - ❌ Name field is highlighted in red with error text below
   - ✅ Form data is NOT cleared (code "USA" is still there)
   - ✅ Cursor automatically focuses on Name field
   - ✅ User can change name and save again

### Test Case 3: Successful Save After Correction
1. Follow Test Case 1 to get a duplicate code error
2. Change code from "US" to "USA"
3. Click Save
4. **Expected results**:
   - ✅ Save succeeds
   - ✅ Error highlighting clears
   - ✅ Success notification appears
   - ✅ Form closes
   - ✅ Grid refreshes with new country

## Migration to Other Entities

To add this pattern to other entity command handlers and UIs:

### Command Handler Changes:

1. Add using statement: `using HIS.MasterData.Application.Common.Exceptions;`
2. Wrap `SaveChangesAsync()` in try-catch block
3. Catch `DbUpdateException` with constraint checking
4. Return specific error messages based on constraint names

### UI Changes:

1. **Add field tracking variables**:
   ```csharp
   private Dictionary<string, string> _fieldErrors = new();
   private MudTextField<string>? _codeField;
   private MudTextField<string>? _nameField;
   ```

2. **Add field references and error properties** to MudTextField components:
   ```razor
   <MudTextField @ref="_codeField" 
                 Label="Code" 
                 @bind-Value="_editModel.Code" 
                 For="@(() => _editModel.Code)" 
                 Error="@_fieldErrors.ContainsKey("Code")" 
                 ErrorText="@GetFieldError("Code")" />
   ```

3. **Add helper methods**:
   - `GetFieldError(string fieldName)`
   - `ClearFieldErrors()`
   - `SetFieldError(string errorMessage)` - customize field parsing logic

4. **Update SubmitForm**:
   - Call `ClearFieldErrors()` before validation
   - Call `SetFieldError(errors)` in error handler
   - Only reload data if form is closed (successful save)

5. **Update SaveAndNew**:
   - Call `ClearFieldErrors()` before validation
   - Return success boolean from Match
   - Only clear `_editModel` if successful
   - Only reload data if successful
   - Call `SetFieldError(errors)` in error handler

6. **Update CloseForm**:
   - Call `ClearFieldErrors()` when closing

7. **Customize field error parsing** in `SetFieldError()` method based on your entity's unique constraints

### Example: Cities Entity

For cities with unique constraints on `Code+CountryId` and `Name+CountryId`:

```csharp
private void SetFieldError(string errorMessage)
{
    ClearFieldErrors();
    
    if (errorMessage.Contains("code '", StringComparison.OrdinalIgnoreCase))
    {
        _fieldErrors["Code"] = errorMessage;
        _codeField?.FocusAsync();
    }
    else if (errorMessage.Contains("name '", StringComparison.OrdinalIgnoreCase))
    {
        _fieldErrors["Name"] = errorMessage;
        _nameField?.FocusAsync();
    }
}
```
