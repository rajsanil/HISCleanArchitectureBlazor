# Plan: Modular HIS Plugin Architecture

**TL;DR**: Restructure into a **modular monolith** with separate class library assemblies per HIS module, a shared `ApplicationDbContext` with dynamic entity/configuration registration, and a **license key system** (RSA-signed JSON) that gates module activation at runtime. Each module follows the existing Clean Architecture pattern (Domain/Application/Infrastructure/UI layers) and self-registers via an `IHisModule` interface. The current monolithic Server.UI remains the host, dynamically loading only licensed module assemblies.

---

## Module Inventory

Based on entity dependencies discovered and your requirements, here are the **13 modules** plus 2 foundation layers:

| # | Module ID | Assembly Prefix | Contains | Dependencies |
|---|-----------|----------------|----------|--------------|
| 0 | `HIS.Core` | `HIS.Core.*` | Module interfaces, shared abstractions, license system | Domain only |
| 1 | `HIS.MasterData` | `HIS.MasterData.*` | Country, City, Nationality, BloodGroup, MaritalStatus | HIS.Core |
| 2 | `HIS.Foundation` | `HIS.Foundation.*` | Facility, Department, Specialty, Location, Room, Bed, Staff | HIS.Core, HIS.MasterData |
| 3 | `HIS.Patient` | `HIS.Patient.*` | Patient, PatientContact, Emirates ID Reader | HIS.Foundation, HIS.MasterData |
| 4 | `HIS.Outpatient` | `HIS.Outpatient.*` | OP Visit, OP Consultation, OP Billing | HIS.Patient, HIS.Foundation |
| 5 | `HIS.Inpatient` | `HIS.Inpatient.*` | Admission, Transfer, Discharge, IP Billing, Bed Board | HIS.Patient, HIS.Foundation |
| 6 | `HIS.Emergency` | `HIS.Emergency.*` | Triage, ED Visit, ED Assessment | HIS.Patient, HIS.Foundation |
| 7 | `HIS.Clinical` | `HIS.Clinical.*` | EHR/EMR, Encounter, Clinical Notes, Orders | HIS.Patient |
| 8 | `HIS.Laboratory` | `HIS.Laboratory.*` | Lab Orders, Specimens, Results | HIS.Clinical, HIS.Patient |
| 9 | `HIS.Radiology` | `HIS.Radiology.*` | Rad Orders, Studies, DICOM, Reports | HIS.Clinical, HIS.Patient |
| 10 | `HIS.Pharmacy` | `HIS.Pharmacy.*` | Prescriptions, Dispensing, Drug Master | HIS.Clinical, HIS.Inventory |
| 11 | `HIS.Nursing` | `HIS.Nursing.*` | Nursing Assessment, Care Plans, Vitals | HIS.Clinical, HIS.Inpatient |
| 12 | `HIS.Inventory` | `HIS.Inventory.*` | Stock, Purchase Orders, Suppliers | HIS.Foundation |
| 13 | `HIS.Insurance` | `HIS.Insurance.*` | Insurance Plans, Claims, Pre-auth | HIS.Patient |
| 14 | `HIS.MRD` | `HIS.MRD.*` | Medical Records, Coding, ICD | HIS.Clinical, HIS.Patient |
| 15 | `HIS.Support` | `HIS.Support.*` | Laundry, Dialysis, Physiotherapy | HIS.Patient, HIS.Foundation |

**Module Dependency Graph:**

```
HIS.Core ← HIS.MasterData ← HIS.Foundation ← HIS.Patient ← ┬── HIS.Outpatient
                                                               ├── HIS.Inpatient ← HIS.Nursing
                                                               ├── HIS.Emergency
                                                               ├── HIS.Clinical ← ┬── HIS.Laboratory
                                                               │                  ├── HIS.Radiology
                                                               │                  ├── HIS.MRD
                                                               │                  └── HIS.Pharmacy ← HIS.Inventory
                                                               ├── HIS.Insurance
                                                               └── HIS.Support
```

---

## Steps

### Phase 1: Core Module Infrastructure (Foundation)

1. **Create `IHisModule` interface** in a new project `src/Modules/HIS.Core/HIS.Core.Abstractions/`
   - Properties: `ModuleId`, `DisplayName`, `Version`, `Dependencies` (string array of required module IDs)
   - Methods: `ConfigureDomainServices(IServiceCollection)`, `ConfigureApplicationServices(IServiceCollection)`, `ConfigureInfrastructureServices(IServiceCollection, IConfiguration)`, `ConfigureUIServices(IServiceCollection)`, `ConfigureDatabase(ModelBuilder)`, `GetMenuSections()` returns menu items, `GetPermissionTypes()` returns permission classes

2. **Create `IModuleDbContext` interface** in `HIS.Core.Abstractions` — defines a contract for modules to register their `DbSet<T>` entries and `IEntityTypeConfiguration<T>` classes without modifying the central `ApplicationDbContext` directly. The host `ApplicationDbContext` will call `builder.ApplyConfigurationsFromAssembly()` for each active module assembly.

3. **Create License Key System** in `src/Modules/HIS.Core/HIS.Core.Licensing/`
   - `LicensePayload` class: `CustomerName`, `ExpiryDate`, `LicensedModules` (string[]), `MaxUsers`, `Signature`
   - `LicenseValidator` service: reads license file from `wwwroot/license.key` or `appsettings.json` path, verifies RSA signature, caches result
   - `ILicenseService` interface in Abstractions: `IsModuleLicensed(string moduleId)`, `GetLicensedModules()`, `GetLicenseInfo()`
   - `LicenseKeyGenerator` console tool (separate project) for your team to generate license keys per customer

4. **Create `ModuleLoader` service** in `HIS.Core.Abstractions`
   - Discovers all `IHisModule` implementations from referenced assemblies via `Assembly.GetEntryAssembly().GetReferencedAssemblies()` or explicit assembly scanning
   - Validates module dependencies (topological sort)
   - Cross-checks against `ILicenseService.GetLicensedModules()`
   - Only activates licensed modules whose dependencies are also licensed

### Phase 2: Solution Structure Reorganization

5. **Create module folder structure** under `src/Modules/`:

   ```
   src/Modules/
   ├── HIS.Core/
   │   ├── HIS.Core.Abstractions/      (IHisModule, ILicenseService, IModuleContext)
   │   └── HIS.Core.Licensing/         (LicenseValidator, LicensePayload)
   ├── HIS.MasterData/
   │   ├── HIS.MasterData.Domain/      (BloodGroup, MaritalStatus, Nationality, Country, City entities)
   │   ├── HIS.MasterData.Application/ (Features: Commands, Queries, DTOs, Validators, Caching)
   │   ├── HIS.MasterData.Infrastructure/ (EF Configurations, Permissions)
   │   └── HIS.MasterData.UI/          (Razor pages, dialogs, module menu registration)
   ├── HIS.Foundation/
   │   ├── HIS.Foundation.Domain/
   │   ├── HIS.Foundation.Application/
   │   ├── HIS.Foundation.Infrastructure/
   │   └── HIS.Foundation.UI/
   ├── HIS.Patient/
   │   └── ... (same 4-layer pattern)
   └── ... (each module follows the same pattern)
   ```

6. **Each module assembly contains a `Module` class** implementing `IHisModule`:

   ```csharp
   // HIS.MasterData.Infrastructure/MasterDataModule.cs
   public class MasterDataModule : IHisModule
   {
       public string ModuleId => "HIS.MasterData";
       public string DisplayName => "Master Data";
       public string Version => "1.0.0";
       public string[] Dependencies => []; // none, it's a base module

       public void ConfigureDatabase(ModelBuilder builder)
           => builder.ApplyConfigurationsFromAssembly(typeof(MasterDataModule).Assembly);

       public IEnumerable<MenuSectionModel> GetMenuSections()
           => [/* Master Data menu items */];

       public IEnumerable<Type> GetPermissionTypes()
           => [typeof(MasterDataPermissions)];
   }
   ```

7. **Migrate existing entities** from monolithic projects into module assemblies:
   - Move entities from `src/Domain/Entities/` to respective module Domain projects
   - Move features from `src/Application/Features/` to respective module Application projects
   - Move EF configurations from `src/Infrastructure/Persistence/Configurations/` to module Infrastructure projects
   - Move permissions from `src/Infrastructure/PermissionSet/` to module Infrastructure projects
   - Move pages from `src/Server.UI/Pages/` to module UI projects (Razor Class Libraries)

### Phase 3: Host Application Modifications

8. **Update `ApplicationDbContext`** in `src/Infrastructure/Persistence/ApplicationDbContext.cs`:
   - Keep core/platform DbSets (Tenant, Logger, AuditTrail, Document, PicklistSet, DataProtectionKey)
   - Remove HIS-specific DbSets (they move to module assemblies)
   - In `OnModelCreating`, iterate active modules and call `module.ConfigureDatabase(builder)` — this applies each module's EF configurations and entity mappings dynamically
   - Module entities are accessed through `DbContext.Set<T>()` rather than typed DbSet properties

9. **Update `IApplicationDbContext`** in `src/Application/Common/Interfaces/IApplicationDbContext.cs`:
   - Keep core DbSets only
   - Each module defines its own `IModuleDbContext` extension or uses `DbContext.Set<T>()` directly in its command/query handlers

10. **Update DI registration chain**:
    - `src/Infrastructure/DependencyInjection.cs`: Add `AddHisModules(IConfiguration)` method that discovers, validates, and registers all licensed modules
    - `src/Server.UI/DependencyInjection.cs`: Register module UI services (Razor component assemblies for routing)
    - `src/Application/DependencyInjection.cs`: Scan module Application assemblies for MediatR handlers and FluentValidation validators

11. **Update `MenuService`** in `src/Server.UI/Services/Navigation/MenuService.cs`:
    - Replace hardcoded Hospital menu section with dynamic menu built from `IHisModule.GetMenuSections()`
    - Filter by licensed modules + user permissions
    - Keep Application and Management sections as-is (core platform)

12. **Update Permission auto-discovery** in `src/Infrastructure/DependencyInjection.cs` `AddAuthenticationService`:
    - Currently uses reflection on `Permissions` nested classes
    - Extend to also scan permission types returned by `IHisModule.GetPermissionTypes()` from active modules

13. **Register Razor Class Library assemblies** for routing:
    - Each module UI project is a Razor Class Library (RCL)
    - In `src/Server.UI/Program.cs`, add `app.MapRazorComponents<App>().AddAdditionalAssemblies(moduleAssemblies)` to register module page routes

### Phase 4: License Key Implementation

14. **Create license generation tool** (`tools/LicenseGenerator/`):
    - Console app that takes customer name, expiry date, module list as input
    - Generates RSA-signed JSON license file
    - Store private key securely (never in source control)
    - Public key embedded in `HIS.Core.Licensing` assembly for verification

15. **License validation middleware**:
    - Add middleware in Server.UI that validates license on startup
    - Show license expiry warning 30 days before expiration
    - Block access to unlicensed module routes (return 403)
    - Admin page to view license status and active modules

16. **License file structure**:

    ```json
    {
      "customer": "Al Noor Clinic",
      "issued": "2026-02-08",
      "expires": "2027-02-08",
      "maxUsers": 50,
      "modules": ["HIS.MasterData", "HIS.Foundation", "HIS.Patient", "HIS.Outpatient"],
      "signature": "base64-rsa-signature..."
    }
    ```

### Phase 5: Deployment Profiles

17. **Define deployment profiles** for common customer types:
    - **Clinic**: HIS.Core + MasterData + Foundation + Patient + Outpatient
    - **Day Surgery**: Clinic + Inpatient (limited) + Clinical
    - **Polyclinic**: Clinic + Laboratory + Radiology + Pharmacy
    - **General Hospital**: All modules
    - **Specialty Hospital**: All modules + specialized config

18. **Solution configurations** in the `.slnx` file:
    - Create build configurations (e.g., `Release-Clinic`, `Release-Hospital`) that include/exclude module project references
    - This allows physical exclusion of assemblies from the build output for smaller deployments
    - License system still gates at runtime as a second layer of protection

### Phase 6: Cross-Module Communication

19. **Define cross-module contracts** via shared interfaces in `HIS.Core.Abstractions`:
    - `IPatientLookupService` — used by Outpatient, Inpatient, Emergency to search patients
    - `IStaffLookupService` — used by all clinical modules
    - `IBedManagementService` — used by Inpatient, Emergency, Nursing
    - `IBillingService` — used by Outpatient, Inpatient, Pharmacy
    - `IInventoryService` — used by Pharmacy, Laboratory
    - Modules depend on abstractions, not on each other's concrete types

20. **Use MediatR notifications for cross-module events**:
    - `PatientRegisteredEvent` → Insurance module listens to check eligibility
    - `AdmissionCreatedEvent` → Nursing module creates care plan
    - `OrderCreatedEvent` → Lab/Radiology/Pharmacy modules pick up orders
    - `DischargeCompletedEvent` → Billing module finalizes charges
    - Events defined in `HIS.Core.Abstractions` so any module can publish/subscribe

---

## Verification

- **Build**: Each module compiles independently — `dotnet build src/Modules/HIS.MasterData/HIS.MasterData.Domain/`
- **License**: Unit test that validates license generation, signing, verification, and expired-license rejection
- **Module Loading**: Integration test that starts the host with a test license containing only `[HIS.MasterData, HIS.Foundation]` and verifies:
  - MasterData/Foundation menus appear
  - Patient/Outpatient menus are hidden
  - Patient routes return 403
  - Only licensed module handlers are registered in MediatR
- **Database**: Run `dotnet ef migrations add` from the host project — all active module configurations should be picked up
- **Deployment**: Build with `Release-Clinic` config and verify the output folder contains only Clinic-related module DLLs

---

## Decisions

- **Single DbContext over per-module DbContext**: Chosen because cross-module FK relationships (Patient → BloodGroup, Visit → Staff) require a unified model. Modules contribute configurations but don't own separate contexts.
- **Razor Class Libraries over embedded pages**: Module UI projects are RCLs so they can be added/removed as assembly references without touching the host project.
- **RSA-signed license over simple config**: Prevents tampering; customers can't self-enable modules by editing a config file.
- **MediatR notifications for cross-module communication over direct service calls**: Maintains loose coupling — modules don't need to reference each other.
- **Gradual migration**: Extract one module at a time (start with HIS.MasterData as it has no dependencies), keeping the existing monolithic structure working until all modules are extracted.
- **`DbContext.Set<T>()` over typed DbSet properties for module entities**: Avoids the host DbContext needing to know about every module entity at compile time.

---

# Implementation Plan: HIS.Core.UI Shared Library & Dependency Fixes

**Created**: Post-MasterData migration analysis  
**Status**: IN PROGRESS (HIS.Core.UI.csproj skeleton created)  
**Goal**: Resolve 231 compilation errors in MasterData.UI by extracting shared UI components into a reusable `HIS.Core.UI` Razor Class Library, fix inverted dependency from Infrastructure → HIS.MasterData.Infrastructure, and clean up Telerik version mismatch.

---

## Problem Statement

After migrating Beds, BedBoard, Contacts, Departments, Locations, Specialties to the MasterData module, 231 compilation errors remain in `HIS.MasterData.UI`. The root cause: module UI pages depend on 7 shared types defined in `Server.UI` and `Infrastructure`, but referencing `Server.UI` from a module creates a **circular dependency**.

### 7 Critical Shared Dependencies

| # | Type | Current Location | Used By Module Pages |
|---|------|-----------------|---------------------|
| 1 | `DialogServiceHelper` | `Server.UI/Services/DialogServiceHelper.cs` | All 6 entity pages (delete confirmation) |
| 2 | `ConstantString` (static class) | `Infrastructure/Constants/ConstantString.cs` | All pages (localized UI strings) |
| 3 | `MudLoadingButton` | `Server.UI/Components/LoadingButton/MudLoadingButton.razor` | All pages (save buttons) |
| 4 | `ReadOnlyFieldX6` | `Server.UI/Components/Forms/ReadOnlyFieldX6.razor` | Dialog forms |
| 5 | `DeleteConfirmation` | `Server.UI/Components/Dialogs/DeleteConfirmation.razor` | Used by DialogServiceHelper |
| 6 | `ConfirmationDialog` | `Server.UI/Components/Dialogs/ConfirmationDialog.razor` | Used by DialogServiceHelper |
| 7 | `SharedResources` marker | Does NOT exist (singular `SharedResource` in Server.UI/Models/) | MasterData _Imports.razor (broken) |

**Additional issues:**
- `PicklistAutocomplete<T>` — depends on `IPicklistService` from core Application; too coupled to move
- `ActiveUserSession` — depends on ActualLab.Fusion; too coupled to move
- `Breadcrumbs` — depends on JS interop, ConstantString, MudLoadingButton; complex to move
- Telerik version mismatch: MasterData.UI uses **7.1.0**, Server.UI uses **12.3.0**
- Infrastructure.csproj references HIS.MasterData.Infrastructure (inverted dependency)

---

## Current Dependency Graph (BROKEN)

```
Domain (no refs)
Application → Domain
HIS.Core.Abstractions (no refs)
HIS.Core.Licensing → HIS.Core.Abstractions
HIS.Core.Infrastructure → HIS.Core.Abstractions, HIS.Core.Licensing
HIS.MasterData.Domain → Domain, HIS.Core.Abstractions
HIS.MasterData.Application → Application, HIS.MasterData.Domain, HIS.Core.Abstractions
HIS.MasterData.Infrastructure → HIS.MasterData.Application, HIS.MasterData.Domain, HIS.Core.Abstractions, HIS.Core.Licensing
HIS.MasterData.UI → HIS.MasterData.Application, HIS.MasterData.Infrastructure, HIS.Core.Abstractions
    ❌ Missing: shared UI components (ConstantString, DialogServiceHelper, MudLoadingButton, etc.)
Infrastructure → Application, HIS.Core.*, HIS.MasterData.Infrastructure  ← ❌ INVERTED DEPENDENCY
Server.UI → Everything (composition root)
```

---

## Target Dependency Graph (FIXED)

```
Domain (no refs)
Application → Domain
HIS.Core.Abstractions (no refs)
HIS.Core.Licensing → HIS.Core.Abstractions
HIS.Core.Infrastructure → HIS.Core.Abstractions, HIS.Core.Licensing
HIS.Core.UI → HIS.Core.Abstractions, MudBlazor          ← NEW shared UI library
HIS.MasterData.Domain → Domain, HIS.Core.Abstractions
HIS.MasterData.Application → Application, HIS.MasterData.Domain, HIS.Core.Abstractions
HIS.MasterData.Infrastructure → HIS.MasterData.Application, HIS.MasterData.Domain, HIS.Core.Abstractions, HIS.Core.Licensing
HIS.MasterData.UI → HIS.MasterData.Application, HIS.MasterData.Infrastructure, HIS.Core.UI  ← ✅ Gets shared components
Infrastructure → Application, HIS.Core.Abstractions, HIS.Core.Licensing, HIS.Core.Infrastructure  ← ✅ NO module reference
Server.UI → Infrastructure, HIS.Core.UI, HIS.MasterData.* (all), HIS.Core.*  ← composition root handles module registration
```

---

## Implementation Phases

### Phase 1: Populate HIS.Core.UI Shared Library

**Project already created:** `src/Modules/HIS.Core/HIS.Core.UI/HIS.Core.UI.csproj`

#### 1.1 Create `_Imports.razor` for HIS.Core.UI

**File:** `src/Modules/HIS.Core/HIS.Core.UI/_Imports.razor`

```razor
@using Microsoft.AspNetCore.Components
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.Extensions.Localization
@using MudBlazor
@using HIS.Core.UI.Components.LoadingButton
@using HIS.Core.UI.Components.Forms
@using HIS.Core.UI.Components.Dialogs
@using HIS.Core.UI.Services
@using HIS.Core.UI.Constants
```

#### 1.2 Move ConstantString + Resource Files

**Source:** `src/Infrastructure/Constants/ConstantString.cs` + `src/Infrastructure/Resources/Constants/ConstantString.*.resx` (12 files + Designer.cs)

**Target:** `src/Modules/HIS.Core/HIS.Core.UI/Constants/ConstantString.cs` + `src/Modules/HIS.Core/HIS.Core.UI/Resources/Constants/ConstantString.*.resx`

**Changes required:**
- Update namespace from `CleanArchitecture.Blazor.Infrastructure.Constants` to `HIS.Core.UI.Constants`
- Update `ResourceManager` assembly reference in ConstantString.cs to `HIS.Core.UI`
- Update `Designer.cs` namespace
- Keep originals in Infrastructure as thin wrappers OR update all Server.UI references to new namespace

**Decision: COPY, don't move.** Keep the original in Infrastructure for backward compatibility with core platform pages. The module UI will use the HIS.Core.UI copy. Eventually, Server.UI pages can migrate to use the HIS.Core.UI version too.

#### 1.3 Move Simple Components

| Component | Source | Target (in HIS.Core.UI) |
|-----------|--------|------------------------|
| `MudLoadingButton.razor` | `Server.UI/Components/LoadingButton/` | `Components/LoadingButton/` |
| `ReadOnlyFieldX6.razor` | `Server.UI/Components/Forms/` | `Components/Forms/` |
| `DeleteConfirmation.razor` | `Server.UI/Components/Dialogs/` | `Components/Dialogs/` |
| `ConfirmationDialog.razor` | `Server.UI/Components/Dialogs/` | `Components/Dialogs/` |

**Changes required:**
- Update component namespaces (implicit from folder path in Razor SDk)
- MudLoadingButton uses `@using MudBlazor` and `IStringLocalizer` — self-contained, safe to move
- DeleteConfirmation and ConfirmationDialog are simple MudDialog wrappers — safe to move
- ReadOnlyFieldX6 is a pure layout component — safe to move

#### 1.4 Move DialogServiceHelper

**Source:** `Server.UI/Services/DialogServiceHelper.cs`
**Target:** `HIS.Core.UI/Services/DialogServiceHelper.cs`

**Changes required:**
- Update namespace from `CleanArchitecture.Blazor.Server.UI.Services` to `HIS.Core.UI.Services`
- Update dialog component references to use HIS.Core.UI.Components.Dialogs namespace
- DialogServiceHelper references `DeleteConfirmation` and `ConfirmationDialog` — both moved in 1.3 ✅

#### 1.5 Create SharedResources Marker Class

**File:** `src/Modules/HIS.Core/HIS.Core.UI/Models/SharedResources.cs`

```csharp
namespace HIS.Core.UI.Models;

/// <summary>
/// Marker class for IStringLocalizer<SharedResources> used across module UIs.
/// Maps to ConstantString resource files via shared culture convention.
/// </summary>
public class SharedResources { }
```

**Note:** This is a localization marker class. The actual translations are in the ConstantString .resx files. The MasterData.UI _Imports.razor injects `IStringLocalizer<SharedResources>` as `ConstantString` — but the module pages actually use the static `ConstantString` class methods, not the localizer. We need to verify which pattern is actually used in each page and fix accordingly.

#### 1.6 Update HIS.Core.UI.csproj (if needed)

Add any additional package references discovered during component migration. Current packages:
- `MudBlazor 8.15.0` ✅
- `MediatR.Contracts 2.0.1` ✅ (if any component uses ISender)
- `Microsoft.AspNetCore.Components.Web 10.0.2` ✅
- May need: `Microsoft.Extensions.Localization` for IStringLocalizer

---

### Phase 2: Update Project References

#### 2.1 Add HIS.Core.UI to HIS.MasterData.UI.csproj

```xml
<!-- ADD -->
<ProjectReference Include="..\..\HIS.Core\HIS.Core.UI\HIS.Core.UI.csproj" />
```

#### 2.2 Add HIS.Core.UI to Server.UI.csproj

```xml
<!-- ADD -->
<ProjectReference Include="..\Modules\HIS.Core\HIS.Core.UI\HIS.Core.UI.csproj" />
```

#### 2.3 Remove Infrastructure → HIS.MasterData.Infrastructure Reference

In `src/Infrastructure/Infrastructure.csproj`, remove:
```xml
<!-- REMOVE -->
<ProjectReference Include="..\Modules\HIS.MasterData\HIS.MasterData.Infrastructure\HIS.MasterData.Infrastructure.csproj" />
```

#### 2.4 Move Module Registration from Infrastructure to Server.UI

**From:** `src/Infrastructure/DependencyInjection.cs` — the `AddHisModules()` method and `IMasterDataDbContext` registration  
**To:** `src/Server.UI/DependencyInjection.cs` or a new file `src/Server.UI/Extensions/ModuleRegistrationExtensions.cs`

This is critical because:
- Infrastructure should NOT know about specific module implementations
- Server.UI is the composition root and already references all module projects
- Module DbContext registrations (e.g., `IMasterDataDbContext`) belong in the composition root

**Code to move:**
```csharp
// From Infrastructure/DependencyInjection.cs — MOVE to Server.UI
services.AddScoped<IMasterDataDbContext>(sp =>
{
    var dbContext = sp.GetRequiredService<ApplicationDbContext>();
    return dbContext;
});

// AddHisModules() method — MOVE to Server.UI
```

#### 2.5 Add HIS.Core.UI to Solution File

Add project entry to `CleanArchitecture.Blazor.slnx`:
```
src/Modules/HIS.Core/HIS.Core.UI/HIS.Core.UI.csproj
```

---

### Phase 3: Fix Module UI Imports

#### 3.1 Update HIS.MasterData.UI/_Imports.razor

**Current (broken):**
```razor
@using CleanArchitecture.Blazor.Application.Common.Interfaces
@using CleanArchitecture.Blazor.Application.Common.Security
@using HIS.MasterData.Infrastructure.Permissions
@inject IStringLocalizer<SharedResources> ConstantString
```

**Fixed:**
```razor
@using CleanArchitecture.Blazor.Application.Common.Interfaces
@using CleanArchitecture.Blazor.Application.Common.Security
@using HIS.MasterData.Infrastructure.Permissions
@using HIS.Core.UI.Constants
@using HIS.Core.UI.Components.LoadingButton
@using HIS.Core.UI.Components.Forms
@using HIS.Core.UI.Components.Dialogs
@using HIS.Core.UI.Services
@* Remove broken IStringLocalizer<SharedResources> injection *@
@* ConstantString is a static class, not injected *@
```

**Key change:** Remove `@inject IStringLocalizer<SharedResources> ConstantString` — the module pages use `ConstantString.Save` / `ConstantString.Delete` etc. which are **static** properties from the `ConstantString` class, not `IStringLocalizer` methods. The static class uses `ResourceManager` internally.

#### 3.2 Update Server.UI/_Imports.razor

Add HIS.Core.UI namespaces (Server.UI will use both its own components and shared ones):
```razor
@using HIS.Core.UI.Constants
@using HIS.Core.UI.Components.LoadingButton
@using HIS.Core.UI.Components.Forms
@using HIS.Core.UI.Components.Dialogs
@using HIS.Core.UI.Services
```

If we COPY (not move) components, Server.UI can keep its existing `@using` lines unchanged. If we MOVE, update the old namespace references.

**Decision: COPY approach** — duplicate the components in HIS.Core.UI. Server.UI keeps originals for now. This avoids breaking existing Server.UI pages. In a future cleanup, Server.UI pages can migrate to use HIS.Core.UI versions and the Server.UI copies can be deleted.

---

### Phase 4: Fix Telerik Version Mismatch

#### 4.1 Update HIS.MasterData.UI.csproj

Change Telerik from 7.1.0 to 12.3.0 to match Server.UI:
```xml
<!-- CHANGE -->
<PackageReference Include="Telerik.UI.for.Blazor" Version="12.3.0" />
```

**Impact:** Telerik 12.x has breaking API changes from 7.x. May require component markup updates in module pages that use Telerik components (e.g., TelerikGrid configuration changes).

#### 4.2 Verify Telerik API Compatibility

Check all `.razor` files in HIS.MasterData.UI for Telerik component usage and update any deprecated APIs.

---

### Phase 5: Build & Verify

#### 5.1 Build Sequence

```bash
# Step 1: Build shared library first
dotnet build src/Modules/HIS.Core/HIS.Core.UI/HIS.Core.UI.csproj

# Step 2: Build module
dotnet build src/Modules/HIS.MasterData/HIS.MasterData.UI/HIS.MasterData.UI.csproj

# Step 3: Build full solution
dotnet build CleanArchitecture.Blazor.slnx
```

#### 5.2 Expected Outcome

- 0 compilation errors
- No circular dependency warnings (MSB4006)
- All 12+ projects build successfully
- Infrastructure.csproj has NO module references
- HIS.MasterData.UI resolves all shared types from HIS.Core.UI

#### 5.3 Runtime Verification

- Start application, navigate to MasterData pages
- Verify beds, contacts, departments, locations, specialties pages load
- Verify CRUD operations (create, edit, delete with confirmations)
- Verify localized strings appear correctly
- Verify Telerik grids render properly

---

## File Change Summary

| File | Action | Description |
|------|--------|-------------|
| `HIS.Core.UI/_Imports.razor` | CREATE | Shared using/inject statements |
| `HIS.Core.UI/Constants/ConstantString.cs` | CREATE (copy) | Static localization class |
| `HIS.Core.UI/Resources/Constants/*.resx` | CREATE (copy) | 12 resource files + Designer.cs |
| `HIS.Core.UI/Components/LoadingButton/MudLoadingButton.razor` | CREATE (copy) | Loading button component |
| `HIS.Core.UI/Components/Forms/ReadOnlyFieldX6.razor` | CREATE (copy) | Read-only field layout |
| `HIS.Core.UI/Components/Dialogs/DeleteConfirmation.razor` | CREATE (copy) | Delete confirmation dialog |
| `HIS.Core.UI/Components/Dialogs/ConfirmationDialog.razor` | CREATE (copy) | Generic confirmation dialog |
| `HIS.Core.UI/Services/DialogServiceHelper.cs` | CREATE (copy) | Dialog helper service |
| `HIS.Core.UI/Models/SharedResources.cs` | CREATE | Localization marker class |
| `HIS.MasterData.UI/HIS.MasterData.UI.csproj` | EDIT | Add HIS.Core.UI ref, fix Telerik version |
| `HIS.MasterData.UI/_Imports.razor` | EDIT | Add HIS.Core.UI namespaces, fix ConstantString |
| `Server.UI/Server.UI.csproj` | EDIT | Add HIS.Core.UI reference |
| `Infrastructure/Infrastructure.csproj` | EDIT | Remove HIS.MasterData.Infrastructure reference |
| `Infrastructure/DependencyInjection.cs` | EDIT | Remove module registration code |
| `Server.UI/DependencyInjection.cs` | EDIT | Add module registration code |
| `CleanArchitecture.Blazor.slnx` | EDIT | Add HIS.Core.UI project |

**Total: ~25 files changed (16 created, 6 edited, plus resx copies)**

---

## Components NOT Moved (Stay in Server.UI)

These components have deep dependencies on core platform services and will be resolved at runtime when the module UI runs inside the Server.UI host:

| Component | Reason |
|-----------|--------|
| `PicklistAutocomplete<T>` | Depends on `IPicklistService` and `PicklistSetDto` from core Application |
| `ActiveUserSession` | Depends on ActualLab.Fusion (`IComputedState`) |
| `Breadcrumbs` | Depends on JS interop, ConstantString, MudLoadingButton — complex chain |

**Runtime resolution**: Since module RCL assemblies are loaded as additional assemblies in the Blazor app, types from Server.UI are available at runtime even without a compile-time reference. Module pages can use `@inject` for services registered in the host container. However, **Razor components must be resolvable at compile time** — so any Razor component used in module `.razor` files must either be in a referenced project or namespace-imported.

**Workaround for PicklistAutocomplete**: If module pages need it, either:
1. Move `IPicklistService` + `PicklistSetDto` to HIS.Core.UI (preferred)
2. Or create a module-specific autocomplete that uses the module's own service

---

## Risk Assessment

| Risk | Impact | Mitigation |
|------|--------|------------|
| Telerik 7→12 breaking changes | HIGH | Review Telerik migration guide; test all grid pages |
| ConstantString static vs IStringLocalizer confusion | MEDIUM | Audit all module pages; standardize on static class usage |
| Module registration move breaks DI | MEDIUM | Test full startup flow; verify all services resolve |
| Component copy creates maintenance burden | LOW | Document in ADR; plan future consolidation |
| Missing resource files cause runtime errors | MEDIUM | Verify all 12 .resx files copied correctly |
