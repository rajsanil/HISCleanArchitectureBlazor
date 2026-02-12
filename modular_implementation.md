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
