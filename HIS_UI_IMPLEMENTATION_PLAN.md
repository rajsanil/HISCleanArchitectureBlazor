# HIS Blazor UI Implementation Plan

> **Purpose**: Step-by-step agent-executable plan to create all Blazor Server UI pages for the Hospital Information System features.
> **Date**: February 7, 2026
> **Prerequisites**: All Domain entities, EF configs, Application features (Commands/Queries/DTOs/Mappers/Caching), Permissions, and Generator services are already implemented and the solution compiles cleanly.

---

## Architecture Overview

### UI Pattern Used: Pattern A (List Page + Dialog Form)
Every feature gets:
```
src/Server.UI/Pages/{Feature}/
├── {Feature}.razor                        # MudDataGrid list page
└── Components/
    └── {Feature}FormDialog.razor          # MudDialog add/edit form
```

### Reference Template
Copy patterns from `src/Server.UI/Pages/Products/Products.razor` and `src/Server.UI/Pages/Products/Components/ProductFormDialog.razor`.

### Globally Available (via `_Imports.razor`)
All pages automatically have access to:
- **Services**: `Mediator`, `DialogService`, `DialogServiceHelper`, `AuthService`, `Validator`, `Snackbar`, `Navigation`, `JS`, `ApplicationSettings`, `Config`
- **Namespaces**: `MudBlazor`, `MediatR`, `FluentValidation`, `Telerik.Blazor`, `CleanArchitecture.Blazor.Infrastructure.PermissionSet` (all `Permissions.*` classes), `CleanArchitecture.Blazor.Application.Common.Models`, all `Server.UI.Components.*`

---

## Task List (26 files: 25 new + 1 edit)

| # | Task | Files | Priority |
|---|------|-------|----------|
| 1 | Facilities page + dialog | 2 | High |
| 2 | Departments page + dialog | 2 | High |
| 3 | Specialties page + dialog | 2 | High |
| 4 | Locations page + dialog | 2 | High |
| 5 | Rooms page + dialog | 2 | High |
| 6 | Beds page + dialog + status dialog | 3 | High |
| 7 | Staff page + dialog | 2 | High |
| 8 | Patients page + dialog | 2 | Critical |
| 9 | Visits page + 4 dialogs | 5 | Critical |
| 10 | Encounters page + dialog | 2 | High |
| 11 | Bed Board dashboard page | 1 | Medium |
| 12 | Update MenuService.cs navigation | 1 edit | Critical |
| 13 | Build verification | 0 | Critical |

---

## Task 1: Facilities Page

### File: `src/Server.UI/Pages/Facilities/Facilities.razor`

**Directives:**
```razor
@page "/pages/facilities"
@using CleanArchitecture.Blazor.Application.Features.Facilities.DTOs
@using CleanArchitecture.Blazor.Application.Features.Facilities.Mappers
@using CleanArchitecture.Blazor.Application.Features.Facilities.Queries.GetAll
@using CleanArchitecture.Blazor.Application.Features.Facilities.Caching
@using CleanArchitecture.Blazor.Application.Features.Facilities.Commands.AddEdit
@using CleanArchitecture.Blazor.Application.Features.Facilities.Commands.Delete
@using CleanArchitecture.Blazor.Server.UI.Pages.Facilities.Components

@attribute [Authorize(Policy = Permissions.Facilities.View)]
@inject IStringLocalizer<Facilities> L
```

**Page structure:**
1. `<PageTitle>@Title</PageTitle>`
2. `<MudDataGrid>` with `Items="@_items"`, `FixedHeader`, `FixedFooter`, `Height="calc(100vh - 330px)"`, `MultiSelection`, `Hover`
3. **ToolBarContent**: Icon `Icons.Material.Filled.LocalHospital`, title from `L[_currentDto.GetClassDescription()]`, Refresh/New/More(Clone,Delete) buttons, Search textfield
4. **Columns**: `<SelectColumn>`, Actions `<TemplateColumn>` with Edit/Delete menu, then `<PropertyColumn>` for:
   - `Code`, `Name`, `NameArabic`, `LicenseNumber`, `Phone`, `Email`
5. **NoRecordsContent** / **LoadingContent**: `<MudDataGirdStatus>` (note existing typo in codebase)
6. **PagerContent**: `<MudDataGridPager PageSizeOptions="@(new[] { 10, 15, 30, 50, 100 })" />`

**@code block:**
- State: `_selectedItems`, `_table`, `_currentDto`, `_loading`, `_defaultPageSize`, permission booleans
- **Uses `GetAllFacilitiesQuery`** (client-side grid with `Items="@_items"`)
- `OnInitializedAsync`: check `Permissions.Facilities.{Create|Search|Edit|Delete}`, call `LoadData()`
- `LoadData()`: `_items = (await Mediator.Send(new GetAllFacilitiesQuery())).ToList();`
- `ShowEditFormDialog(title, command)`: `DialogParameters<FacilityFormDialog>` + `DialogService.ShowAsync`
- `OnCreate`: `FacilityMapper` → new `AddEditFacilityCommand()`
- `OnClone`: `FacilityMapper.CloneFromDto(selected)`
- `OnEdit`: `FacilityMapper.ToEditCommand(dto)`
- `OnDelete`: `DeleteFacilityCommand(new[] { dto.Id })` via `DialogServiceHelper.ShowDeleteConfirmationDialogAsync`
- `OnRefresh`: `FacilityCacheKey.Refresh(); await LoadData();`

### File: `src/Server.UI/Pages/Facilities/Components/FacilityFormDialog.razor`

**Form fields (MudGrid xs=12 sm=6):**
- `Code` (MudTextField, Required)
- `Name` (MudTextField, Required)
- `NameArabic` (MudTextField)
- `LicenseNumber` (MudTextField)
- `Address` (MudTextField, Lines=2, xs=12)
- `Phone` (MudTextField)
- `Email` (MudTextField)

**@code:** Standard dialog pattern with `AddEditFacilityCommand Model`, `Submit`, `SaveAndNew`, `Cancel`.

---

## Task 2: Departments Page

### File: `src/Server.UI/Pages/Departments/Departments.razor`

Same pattern as Facilities with:
- **Route**: `/pages/departments`
- **Permission**: `Permissions.Departments.View`
- **DTO**: `DepartmentDto`, **Query**: `GetAllDepartmentsQuery`
- **Commands**: `AddEditDepartmentCommand` / `DeleteDepartmentCommand`
- **Mapper**: `DepartmentMapper`, **CacheKey**: `DepartmentCacheKey`
- **Columns**: `Code`, `Name`, `NameArabic`, `FacilityId`, `ParentDepartmentId`, `IsActive`
- **Icon**: `Icons.Material.Filled.AccountTree`

### File: `src/Server.UI/Pages/Departments/Components/DepartmentFormDialog.razor`

**Form fields:** `Code`, `Name`, `NameArabic`, `FacilityId` (dropdown from `GetAllFacilitiesQuery`, Required), `ParentDepartmentId` (dropdown from `GetAllDepartmentsQuery`, Optional — exclude self), `IsActive` (MudCheckBox)

**The dialog loads dropdown data in `OnInitializedAsync`:**
```csharp
_facilities = await Mediator.Send(new GetAllFacilitiesQuery());
_departments = await Mediator.Send(new GetAllDepartmentsQuery());
```

---

## Task 3: Specialties Page

### File: `src/Server.UI/Pages/Specialties/Specialties.razor`

- **Route**: `/pages/specialties`, **Permission**: `Permissions.Departments.View`
- **DTO**: `SpecialtyDto`, **Query**: `GetAllSpecialtiesQuery`
- **Commands**: `AddEditSpecialtyCommand` / `DeleteSpecialtyCommand`
- **Columns**: `Code`, `Name`, `NameArabic`, `DepartmentId`, `IsActive`
- **Icon**: `Icons.Material.Filled.MedicalInformation`

### File: `src/Server.UI/Pages/Specialties/Components/SpecialtyFormDialog.razor`

**Form fields:** `Code`, `Name`, `NameArabic`, `DepartmentId` (dropdown), `IsActive`

---

## Task 4: Locations Page

### File: `src/Server.UI/Pages/Locations/Locations.razor`

- **Route**: `/pages/locations`, **Permission**: `Permissions.Departments.View`
- **DTO**: `LocationDto`, **Commands**: `AddEditLocationCommand` / `DeleteLocationCommand`
- **Columns**: `Code`, `Name`, `LocationType`, `DepartmentId`, `FacilityId`, `IsActive`
- **Icon**: `Icons.Material.Filled.LocationOn`

### File: `src/Server.UI/Pages/Locations/Components/LocationFormDialog.razor`

**Form fields:** `Code`, `Name`, `LocationType` (PicklistAutocomplete `Picklist.LocationType`), `FacilityId` (dropdown), `DepartmentId` (dropdown), `IsActive`

---

## Task 5: Rooms Page

### File: `src/Server.UI/Pages/Rooms/Rooms.razor`

- **Route**: `/pages/rooms`, **Permission**: `Permissions.Departments.View`
- **DTO**: `RoomDto`, **Commands**: `AddEditRoomCommand` / `DeleteRoomCommand`
- **Columns**: `Code`, `Name`, `RoomType`, `LocationId`, `IsActive`
- **Icon**: `Icons.Material.Filled.MeetingRoom`

### File: `src/Server.UI/Pages/Rooms/Components/RoomFormDialog.razor`

**Form fields:** `Code`, `Name`, `RoomType` (PicklistAutocomplete `Picklist.RoomType`), `LocationId` (dropdown), `IsActive`

---

## Task 6: Beds Page

### File: `src/Server.UI/Pages/Beds/Beds.razor`

- **Route**: `/pages/beds`, **Permission**: `Permissions.Beds.View`
- **DTO**: `BedDto`, **Commands**: `AddEditBedCommand` / `DeleteBedCommand` / `UpdateBedStatusCommand`
- **Columns**: `Code`, `RoomId`, `BedStatus` (colored MudChip), `IsActive`
- **Icon**: `Icons.Material.Filled.Bed`
- **Extra permission**: `_canManageStatus` from `Permissions.Beds.ManageStatus`
- **Extra row action**: "Change Status" → opens `BedStatusDialog`

**BedStatus chip color map:**
```
Available → Color.Success, Occupied → Color.Error,
Cleaning → Color.Warning, Maintenance → Color.Default,
Reserved → Color.Info, Blocked → Color.Dark
```

### File: `src/Server.UI/Pages/Beds/Components/BedFormDialog.razor`

**Form fields:** `Code` (Required), `RoomId` (dropdown from `GetAllRoomsQuery`, Required), `BedStatus` (MudSelect with 6 values), `IsActive`

### File: `src/Server.UI/Pages/Beds/Components/BedStatusDialog.razor`

Small dialog — `[Parameter] int BedId`, `MudSelect<string>` for status, sends `UpdateBedStatusCommand { BedId, NewStatus }`.

---

## Task 7: Staff Page

### File: `src/Server.UI/Pages/StaffMembers/StaffMembers.razor`

- **Route**: `/pages/staff`, **Permission**: `Permissions.StaffMembers.View`
- **DTO**: `StaffDto`, **Commands**: `AddEditStaffCommand` / `DeleteStaffCommand`
- **Columns**: `EmployeeCode`, `FullName`, `StaffType`, `DepartmentId`, `LicenseNumber`, `IsActive`
- **Icon**: `Icons.Material.Filled.Badge`

### File: `src/Server.UI/Pages/StaffMembers/Components/StaffFormDialog.razor`

**Form fields:** `EmployeeCode`, `Title`, `FirstName`, `LastName`, `StaffType` (PicklistAutocomplete `Picklist.StaffType`), `DepartmentId` (dropdown), `SpecialtyId` (dropdown), `LicenseNumber`, `ApplicationUserId` (PickUserAutocomplete), `IsActive`

---

## Task 8: Patients Page (CRITICAL)

### File: `src/Server.UI/Pages/Patients/Patients.razor`

- **Route**: `/pages/patients`, **Permission**: `Permissions.Patients.View`
- **Uses SERVER-SIDE pagination**: `PatientsWithPaginationQuery`
- **GridData via `ServerReload`** (same pattern as Products.razor)
- **Columns**: `MRN`, FullName (First+Last), `Gender`, `DateOfBirth`, `EmiratesId`, `Phone`, `IsVIP` (conditional MudChip), `IsActive`

**ServerReload maps GridState → Query:**
```csharp
Query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? "Id";
Query.SortDirection = ... ;
Query.PageNumber = state.Page + 1;
Query.PageSize = state.PageSize;
var result = await Mediator.Send(Query);
return new GridData<PatientDto> { TotalItems = result.TotalItems, Items = result.Items };
```

### File: `src/Server.UI/Pages/Patients/Components/PatientFormDialog.razor`

**Large form organized into sections with `<MudText Typo="Typo.subtitle1">`:**

**Section 1 — Personal Information:**
`FirstName`, `MiddleName`, `LastName`, `FirstNameArabic`, `LastNameArabic`, `DateOfBirth` (MudDatePicker), `Gender` (PicklistAutocomplete), `BloodGroup` (PicklistAutocomplete), `MaritalStatus` (PicklistAutocomplete), `NationalityCode`, `IsVIP` (MudCheckBox)

**Section 2 — Identity Documents:**
`EmiratesId`, `PassportNumber`

**Section 3 — Contact Information:**
`Phone`, `Email`, `Address` (Lines=2), `City`, `Country`

**Section 4 — MRN (read-only on edit):**
Show `MRN` as read-only `MudTextField` when `Model.Id > 0`

**Dialog MaxWidth**: `MaxWidth.Medium`

---

## Task 9: Visits Page (CRITICAL — most complex)

### File: `src/Server.UI/Pages/Visits/Visits.razor`

- **Route**: `/pages/visits`, **Permission**: `Permissions.Visits.View`
- **Uses SERVER-SIDE pagination**: `VisitsWithPaginationQuery`
- **Columns**: `VisitNumber`, PatientName, `VisitType`, `VisitStatus` (colored chip), `FacilityId`, `RegistrationDate`, `DischargeDate`

**VisitStatus chip colors:**
```
Registered → Color.Info, Admitted → Color.Primary,
Discharged → Color.Success, Cancelled → Color.Default
```

**Extra permission flags:**
`_canCancel` (Permissions.Visits.Cancel), `_canAdmit` (Permissions.Visits.Admit), `_canTransfer` (Permissions.Visits.Transfer), `_canDischarge` (Permissions.Visits.Discharge)

**Context-sensitive row actions based on VisitStatus:**
- Edit: visible when not Discharged/Cancelled
- Admit: visible when `Registered`
- Transfer: visible when `Admitted`
- Discharge: visible when `Admitted` or `Registered`
- Cancel: visible when not Discharged/Cancelled

**Action methods open respective dialogs:**
- `OnCreate/OnEdit` → `RegisterVisitFormDialog`
- `OnAdmit(dto)` → `AdmitPatientDialog` with `VisitId = dto.Id`
- `OnTransfer(dto)` → `TransferPatientDialog` with `VisitId = dto.Id`
- `OnDischarge(dto)` → `DischargePatientDialog` with `VisitId = dto.Id`
- `OnCancel(dto)` → Confirmation dialog → `CancelVisitCommand`

### File: `src/Server.UI/Pages/Visits/Components/RegisterVisitFormDialog.razor`

**Fields:** `PatientId` (MudNumericField), `VisitType` (PicklistAutocomplete), `FacilityId` (dropdown), `DepartmentId` (dropdown), `AttendingDoctorId` (dropdown from staff)

**Command**: `RegisterVisitCommand`

### File: `src/Server.UI/Pages/Visits/Components/AdmitPatientDialog.razor`

**Fields:** `LocationId`, `RoomId`, `BedId` (filter to Available beds), `AdmittingDoctorId`, `AdmissionType` (MudSelect: Elective/Emergency), `ExpectedDischargeDate` (MudDatePicker)

**Parameter**: `[Parameter] int VisitId` → sets `_model.VisitId = VisitId`
**Command**: `AdmitPatientCommand`
**Loads dropdowns in `OnInitializedAsync`**

### File: `src/Server.UI/Pages/Visits/Components/TransferPatientDialog.razor`

**Fields:** `ToLocationId`, `ToBedId` (Available beds), `Reason` (Lines=3), `OrderedByDoctorId`

**Parameter**: `int VisitId`, **Command**: `TransferPatientCommand`

### File: `src/Server.UI/Pages/Visits/Components/DischargePatientDialog.razor`

**Fields:** `DischargeDisposition` (PicklistAutocomplete), `DischargeSummary` (Lines=4), `DischargedByDoctorId`, `FollowUpDate`, `FollowUpNotes`

**Parameter**: `int VisitId`, **Command**: `DischargePatientCommand`

---

## Task 10: Encounters Page

### File: `src/Server.UI/Pages/Encounters/Encounters.razor`

- **Route**: `/pages/encounters`, **Permission**: `Permissions.Encounters.View`
- **Client-side grid**: `GetAllEncountersQuery`
- **Columns**: `EncounterNumber`, `VisitId`, `EncounterType`, `EncounterStatus` (chip), `DoctorId`, `StartDate`, `EndDate`, `ChiefComplaint`
- **Row actions**: Edit (if not Completed), End Encounter (if InProgress → `EndEncounterCommand`), Delete

**EncounterStatus colors:** Planned→Default, InProgress→Primary, Completed→Success, Cancelled→Warning

### File: `src/Server.UI/Pages/Encounters/Components/EncounterFormDialog.razor`

**Fields:** `VisitId`, `EncounterType` (PicklistAutocomplete), `DoctorId`, `DepartmentId`, `LocationId`, `ChiefComplaint`, `Notes`
**Command**: `StartEncounterCommand`

---

## Task 11: Bed Board Dashboard

### File: `src/Server.UI/Pages/BedBoard/BedBoard.razor`

- **Route**: `/pages/bedboard`, **Permission**: `Permissions.Beds.View`
- **No MudDataGrid** — visual dashboard
- Loads all Locations, Rooms, Beds via their `GetAll` queries
- Groups: Location → Room → Beds
- Each bed rendered as a colored `<MudChip>` inside a `<MudCard>` per room
- Status color legend at the bottom
- Refresh button calls `BedCacheKey.Refresh()` + reloads all

**@code example:**
```csharp
@code {
    private List<LocationDto> _locations = new();
    private List<RoomDto> _rooms = new();
    private List<BedDto> _beds = new();

    protected override async Task OnInitializedAsync() => await LoadData();

    private async Task LoadData()
    {
        BedCacheKey.Refresh();
        _locations = (await Mediator.Send(new GetAllLocationsQuery())).ToList();
        _rooms = (await Mediator.Send(new GetAllRoomsQuery())).ToList();
        _beds = (await Mediator.Send(new GetAllBedsQuery())).ToList();
        StateHasChanged();
    }

    private Color GetBedStatusColor(string status) => status switch
    {
        "Available" => Color.Success,
        "Occupied" => Color.Error,
        "Cleaning" => Color.Warning,
        "Maintenance" => Color.Default,
        "Reserved" => Color.Info,
        "Blocked" => Color.Dark,
        _ => Color.Default
    };
}
```

---

## Task 12: Update Navigation Menu

### File to edit: `src/Server.UI/Services/Navigation/MenuService.cs`

Insert a new `MenuSectionModel` after the "Application" section:

```csharp
new MenuSectionModel
{
    Title = "Hospital",
    SectionItems = new List<MenuSectionItemModel>
    {
        new()
        {
            Title = "Foundation",
            Icon = Icons.Material.Filled.LocalHospital,
            IsParent = true,
            PageStatus = PageStatus.Completed,
            MenuItems = new List<MenuSectionSubItemModel>
            {
                new() { Title = "Facilities", Href = "/pages/facilities", PageStatus = PageStatus.Completed },
                new() { Title = "Departments", Href = "/pages/departments", PageStatus = PageStatus.Completed },
                new() { Title = "Specialties", Href = "/pages/specialties", PageStatus = PageStatus.Completed },
                new() { Title = "Locations", Href = "/pages/locations", PageStatus = PageStatus.Completed },
                new() { Title = "Rooms", Href = "/pages/rooms", PageStatus = PageStatus.Completed },
                new() { Title = "Beds", Href = "/pages/beds", PageStatus = PageStatus.Completed },
                new() { Title = "Staff", Href = "/pages/staff", PageStatus = PageStatus.Completed }
            }
        },
        new()
        {
            Title = "Patients",
            Icon = Icons.Material.Filled.PersonSearch,
            IsParent = true,
            PageStatus = PageStatus.Completed,
            MenuItems = new List<MenuSectionSubItemModel>
            {
                new() { Title = "Patient Registry", Href = "/pages/patients", PageStatus = PageStatus.Completed },
                new() { Title = "Bed Board", Href = "/pages/bedboard", PageStatus = PageStatus.Completed }
            }
        },
        new()
        {
            Title = "Clinical",
            Icon = Icons.Material.Filled.MedicalServices,
            IsParent = true,
            PageStatus = PageStatus.Completed,
            MenuItems = new List<MenuSectionSubItemModel>
            {
                new() { Title = "Visits", Href = "/pages/visits", PageStatus = PageStatus.Completed },
                new() { Title = "Encounters", Href = "/pages/encounters", PageStatus = PageStatus.Completed }
            }
        }
    }
},
```

---

## Task 13: Build Verification

Run: `dotnet build CleanArchitecture.Blazor.slnx`
Expect: `Build succeeded` with 0 errors.

---

## File Summary

| # | File Path | Type |
|---|-----------|------|
| 1 | `src/Server.UI/Pages/Facilities/Facilities.razor` | New |
| 2 | `src/Server.UI/Pages/Facilities/Components/FacilityFormDialog.razor` | New |
| 3 | `src/Server.UI/Pages/Departments/Departments.razor` | New |
| 4 | `src/Server.UI/Pages/Departments/Components/DepartmentFormDialog.razor` | New |
| 5 | `src/Server.UI/Pages/Specialties/Specialties.razor` | New |
| 6 | `src/Server.UI/Pages/Specialties/Components/SpecialtyFormDialog.razor` | New |
| 7 | `src/Server.UI/Pages/Locations/Locations.razor` | New |
| 8 | `src/Server.UI/Pages/Locations/Components/LocationFormDialog.razor` | New |
| 9 | `src/Server.UI/Pages/Rooms/Rooms.razor` | New |
| 10 | `src/Server.UI/Pages/Rooms/Components/RoomFormDialog.razor` | New |
| 11 | `src/Server.UI/Pages/Beds/Beds.razor` | New |
| 12 | `src/Server.UI/Pages/Beds/Components/BedFormDialog.razor` | New |
| 13 | `src/Server.UI/Pages/Beds/Components/BedStatusDialog.razor` | New |
| 14 | `src/Server.UI/Pages/StaffMembers/StaffMembers.razor` | New |
| 15 | `src/Server.UI/Pages/StaffMembers/Components/StaffFormDialog.razor` | New |
| 16 | `src/Server.UI/Pages/Patients/Patients.razor` | New |
| 17 | `src/Server.UI/Pages/Patients/Components/PatientFormDialog.razor` | New |
| 18 | `src/Server.UI/Pages/Visits/Visits.razor` | New |
| 19 | `src/Server.UI/Pages/Visits/Components/RegisterVisitFormDialog.razor` | New |
| 20 | `src/Server.UI/Pages/Visits/Components/AdmitPatientDialog.razor` | New |
| 21 | `src/Server.UI/Pages/Visits/Components/TransferPatientDialog.razor` | New |
| 22 | `src/Server.UI/Pages/Visits/Components/DischargePatientDialog.razor` | New |
| 23 | `src/Server.UI/Pages/Encounters/Encounters.razor` | New |
| 24 | `src/Server.UI/Pages/Encounters/Components/EncounterFormDialog.razor` | New |
| 25 | `src/Server.UI/Pages/BedBoard/BedBoard.razor` | New |
| 26 | `src/Server.UI/Services/Navigation/MenuService.cs` | **Edit** |

---

## Key Patterns Cheat Sheet

### List Page Skeleton (client-side, for features with GetAll query)
```razor
@page "/pages/{feature}"
@using CleanArchitecture.Blazor.Application.Features.{Feature}.DTOs
@using CleanArchitecture.Blazor.Application.Features.{Feature}.Mappers
@using CleanArchitecture.Blazor.Application.Features.{Feature}.Queries.GetAll
@using CleanArchitecture.Blazor.Application.Features.{Feature}.Caching
@using CleanArchitecture.Blazor.Application.Features.{Feature}.Commands.AddEdit
@using CleanArchitecture.Blazor.Application.Features.{Feature}.Commands.Delete
@using CleanArchitecture.Blazor.Server.UI.Pages.{Feature}.Components

@attribute [Authorize(Policy = Permissions.{Feature}.View)]
@inject IStringLocalizer<{Feature}Page> L

<PageTitle>@Title</PageTitle>

<MudDataGrid Items="@_items" FixedHeader="true" FixedFooter="true"
             @bind-RowsPerPage="_defaultPageSize" Height="calc(100vh - 330px)"
             Loading="@_loading" MultiSelection="true"
             @bind-SelectedItems="_selectedItems" @bind-SelectedItem="_currentDto"
             Hover="true" @ref="_table">
    <ToolBarContent>
        <!-- Icon + Title + Refresh/New/More/Search -->
    </ToolBarContent>
    <Columns>
        <SelectColumn ShowInFooter="false" />
        <TemplateColumn HeaderStyle="width:60px" Title="@ConstantString.Actions" Sortable="false">
            <!-- Edit/Delete menu -->
        </TemplateColumn>
        <PropertyColumn Property="x => x.{Prop}" Title="@L[_currentDto.GetMemberDescription(x => x.{Prop})]" />
        <!-- More columns -->
    </Columns>
    <NoRecordsContent><MudDataGirdStatus Icon="@Icons.Material.Outlined.Inbox" Text="@ConstantString.NoRecords" /></NoRecordsContent>
    <LoadingContent><MudDataGirdStatus IsLoading="true" Text="@ConstantString.Loading" /></LoadingContent>
    <PagerContent><MudDataGridPager PageSizeOptions="@(new[] { 10, 15, 30, 50, 100 })" /></PagerContent>
</MudDataGrid>

@code {
    // CascadingParameters, permission flags, LoadData via GetAll query
    // ShowEditFormDialog, OnCreate, OnClone, OnEdit, OnDelete, OnDeleteChecked, OnRefresh
}
```

### List Page Skeleton (server-side, for Patients/Visits with pagination query)
```razor
<MudDataGrid ServerData="@(ServerReload)" ...>
```
Replace `Items` with `ServerData` and implement `ServerReload` method that maps `GridState` → pagination query → `Mediator.Send` → `GridData<TDto>`.

### Form Dialog Skeleton
```razor
@using CleanArchitecture.Blazor.Application.Features.{Feature}.Commands.AddEdit
@inject IStringLocalizer<{Feature}Page> L

<MudDialog>
    <DialogContent>
        <MudForm Model="@Model" @ref="@_form" Validation="@(Validator.ValidateValue(Model))">
            <MudGrid Spacing="2">
                <!-- form fields -->
            </MudGrid>
        </MudForm>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Cancel">@ConstantString.Cancel</MudButton>
        <MudLoadingButton Loading="@_saveingnew" OnClick="SaveAndNew">@ConstantString.SaveAndNew</MudLoadingButton>
        <MudLoadingButton Loading="@_saving" OnClick="Submit">@ConstantString.Save</MudLoadingButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [EditorRequired][Parameter] public AddEdit{Feature}Command Model { get; set; } = default!;
    [Parameter] public Action? Refresh { get; set; }
    private MudForm? _form;
    private bool _saving;
    private bool _saveingnew;

    private async Task Submit() { /* validate → Mediator.Send(Model) → result.Match(close/error) */ }
    private async Task SaveAndNew() { /* validate → send → reset Model */ }
    private void Cancel() => MudDialog.Cancel();
}
```

### Workflow Dialog Skeleton (Admit/Transfer/Discharge — no SaveAndNew)
```razor
<MudDialog>
    <DialogContent>
        <MudForm Model="@_model" @ref="@_form" Validation="@(Validator.ValidateValue(_model))">
            <!-- fields, dropdowns loaded in OnInitializedAsync -->
        </MudForm>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Cancel">@ConstantString.Cancel</MudButton>
        <MudLoadingButton Loading="@_saving" OnClick="Submit">@ConstantString.Save</MudLoadingButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [EditorRequired][Parameter] public int VisitId { get; set; }
    private {CommandType} _model = new();
    // Load dropdown data in OnInitializedAsync, set _model.VisitId = VisitId
    // Submit: validate → Mediator.Send(_model) → result.Match
}
```

---

## Important Notes for the Implementing Agent

1. **Spelling**: The existing shared component is `MudDataGirdStatus` (NOT "Grid") — match exact spelling.
2. **ConstantString**: Use `ConstantString.Refresh`, `.New`, `.Delete`, `.Edit`, `.Search`, `.Cancel`, `.Save`, `.SaveAndNew`, `.SaveSuccess`, `.NoRecords`, `.Loading`, `.Actions`, `.NoAllowed`, `.Selected`, `.CreateAnItem`, `.EditTheItem`, `.DeleteConfirmation`, `.DeleteConfirmationTitle`, `.DeleteConfirmWithSelected`.
3. **Dialog invocation**: `DialogParameters<TDialog>` → `DialogService.ShowAsync<TDialog>` → `await dialog.Result` → check `!state.Canceled`.
4. **Delete pattern**: `DialogServiceHelper.ShowDeleteConfirmationDialogAsync(command, title, contentText, callback)`.
5. **Column titles**: `_currentDto.GetMemberDescription(x => x.Prop)` (reads `[Description]` from DTO).
6. **Page title**: `_currentDto.GetClassDescription()` (reads `[Description]` from DTO class).
7. **PicklistAutocomplete**: For picklist-driven fields — `Picklist.Gender`, `.MaritalStatus`, `.BloodGroup`, `.VisitType`, `.VisitStatus`, `.EncounterType`, `.EncounterStatus`, `.AdmissionType`, `.DischargeDisposition`, `.LocationType`, `.RoomType`, `.BedStatus`, `.StaffType`.
8. **Build after every 2-3 pages** to catch errors early.
9. **No @using needed** for: MudBlazor, MediatR, Permissions, ConstantString, Snackbar, Mediator — globally imported.
10. **Feature-specific @using always needed**: DTOs, Mappers, Queries, Commands, Caching, Components namespaces.

---

## Verification Steps

1. **Build**: `dotnet build CleanArchitecture.Blazor.slnx` → expect 0 errors
2. **Run application**: Launch with F5 or `dotnet run`
3. **Navigate all routes**:
   - /pages/facilities
   - /pages/departments (test parent department dropdown)
   - /pages/specialties
   - /pages/locations (test LocationType picklist)
   - /pages/rooms (test RoomType picklist)
   - /pages/beds (test Change Status dialog)
   - /pages/staff (test StaffType picklist)
   - /pages/patients (test server-side pagination, VIP chip)
   - /pages/visits (test all ADT workflows: Register → Admit → Transfer → Discharge)
   - /pages/encounters (test End Encounter action)
   - /pages/bedboard (verify color-coded bed chips)
4. **Test end-to-end Patient-to-Discharge flow**:
   - Create a new patient → note MRN
   - Register a visit for that patient
   - Admit the visit (select available bed)
   - Transfer to another bed
   - Discharge with follow-up
   - Verify Bed Board shows bed status changes
5. **Test permission-based visibility**: Login with different roles to verify Create/Edit/Delete buttons show/hide correctly

---

## Phase 1 Completion Criteria

✅ All 25 UI files created  
✅ MenuService.cs updated with Hospital section  
✅ Build succeeds with 0 errors  
✅ All pages load without runtime errors  
✅ Patient registration generates MRN automatically  
✅ Visit workflow (Register → Admit → Transfer → Discharge → Cancel) works end-to-end  
✅ Bed Board dashboard displays real-time bed status  
✅ Permission policies enforce access control  

---

**End of Implementation Plan**
