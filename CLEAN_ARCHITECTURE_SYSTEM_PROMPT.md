# Clean Architecture System Prompt for AI Development Assistant

## Project Overview
This is a **Blazor Server application** built with **.NET 10** following **Clean Architecture principles**. The project emphasizes strict layer separation, dependency inversion, and enterprise-grade patterns for maintainable, scalable software development.

---

## Architecture Layers & Dependencies

### Layer Structure (Dependency Flow: UI → Infrastructure → Application → Domain)

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Server.UI     │    │  Application    │    │     Domain      │
│   (Blazor)      │───▶│   (Business)    │───▶│   (Entities)    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                        │                        
         │              ┌─────────────────┐               
         └─────────────▶│ Infrastructure  │               
                        │   (Data/IO)     │               
                        └─────────────────┘               
```

### 1. **Domain Layer** (`src/Domain/`)
**Purpose**: Contains core business entities, domain events, value objects, and interfaces.
**Dependencies**: **NONE** - This is the innermost layer with zero external dependencies.

#### Key Principles:
- **Entity Hierarchy**:
  - **Int Primary Key (Default)**:
    - `BaseEntity`: Abstract base with `Id` (int) and domain events collection
    - `BaseAuditableEntity`: Extends `BaseEntity` with audit fields (Created, CreatedBy, LastModified, LastModifiedBy)
    - `BaseAuditableSoftDeleteEntity`: Adds soft delete capability with `Deleted` timestamp
  - **String Primary Key**:
    - `BaseEntityWithStringKey`: Abstract base with `Id` (string) and domain events collection
    - `BaseAuditableEntityWithStringKey`: Extends `BaseEntityWithStringKey` with audit fields
    - `BaseAuditableSoftDeleteEntityWithStringKey`: Adds soft delete capability with `Deleted` timestamp
  
- **Domain Events**: All entities can raise domain events via `AddDomainEvent()`, `RemoveDomainEvent()`, `ClearDomainEvents()`
  - Built-in events: `CreatedEvent<T>`, `UpdatedEvent<T>`, `DeletedEvent<T>`

- **Primary Key Selection**:
  - Use **int-based** entities for most cases (auto-incrementing IDs)
  - Use **string-based** entities when you need:
    - GUID identifiers for distributed systems
    - Custom formatted IDs (e.g., "ORD-20260205-001", "INV-12345")
    - Natural string keys (e.g., email, username)
    - External system integration with string IDs

- **Interfaces**:
  - `IEntity<TKey>`: Defines entity contract
  - `IAuditableEntity`: Audit tracking contract
  - `ISoftDelete`: Soft delete contract
  - `IMayHaveTenant`: Multi-tenancy contract

#### Structure:
```
Domain/
├── Common/
│   ├── Entities/          # Base entity classes
│   ├── Events/            # Domain event base classes
│   └── Interfaces/        # Core contracts
├── Entities/              # Business entities (Product, Contact, Document, etc.)
├── ValueObjects/          # Immutable value objects
├── Identity/              # Identity entities (ApplicationUser, ApplicationRole)
└── Events/                # Domain-specific events
```

**Rules**:
- ✅ NO dependencies on other layers
- ✅ Pure C# classes - no framework dependencies
- ✅ Rich domain models with business logic
- ✅ Use domain events for cross-entity concerns
- ❌ NO EF Core attributes (use Fluent API in Infrastructure)
- ❌ NO service references
- ❌ NO UI concerns

---

### 2. **Application Layer** (`src/Application/`)
**Purpose**: Contains business logic, use cases, DTOs, validation, interfaces, and orchestration.
**Dependencies**: `Domain` layer only

#### Key Principles:
- **CQRS Pattern** (Command Query Responsibility Segregation):
  - Commands: Modify state, return `Result<T>`
  - Queries: Read-only, return DTOs or collections
  
- **MediatR Pipeline**:
  - All requests/commands go through MediatR
  - Pipeline behaviors: `ValidationPreProcessor`, `PerformanceBehaviour`, `FusionCacheBehaviour`, `CacheInvalidationBehaviour`
  
- **Feature Organization**:
  ```
  Features/
  └── Products/
      ├── Commands/
      │   ├── AddEdit/
      │   │   ├── AddEditProductCommand.cs
      │   │   ├── AddEditProductCommandValidator.cs
      │   │   └── AddEditProductCommandHandler.cs
      │   └── Delete/
      ├── Queries/
      │   ├── GetAll/
      │   ├── Pagination/
      │   └── Export/
      ├── DTOs/
      ├── Mappers/
      ├── Specifications/
      └── Caching/
  ```

- **Validation**: FluentValidation for all commands
- **Specifications**: Query specifications for complex filtering (using Ardalis.Specification)
- **Caching**: Cache keys and invalidation strategies

#### Structure:
```
Application/
├── Common/
│   ├── Interfaces/        # Service contracts (IApplicationDbContext, IMailService, etc.)
│   ├── Models/            # Shared DTOs and models
│   ├── Security/          # Authorization policies
│   ├── PublishStrategies/ # Domain event publishing strategies
│   └── ExceptionHandlers/ # Global exception handling
├── Features/              # Feature-based organization
│   ├── Products/
│   ├── Contacts/
│   ├── Documents/
│   └── Identity/
├── Pipeline/              # MediatR pipeline behaviors
└── Resources/             # Localization resources
```

**Rules**:
- ✅ Define interfaces for external services (email, file storage, etc.)
- ✅ Use DTOs for data transfer, never expose entities
- ✅ Implement `IRequest<TResponse>` for commands/queries
- ✅ Implement `IRequestHandler<TRequest, TResponse>` for handlers
- ✅ Use `INotification` for domain event handlers
- ✅ Validate all commands with FluentValidation
- ✅ Use Specifications pattern for complex queries
- ✅ Implement cache invalidation with `ICacheInvalidatorRequest`
- ❌ NO direct database access (use IApplicationDbContext)
- ❌ NO concrete infrastructure implementations
- ❌ NO UI logic

---

### 3. **Infrastructure Layer** (`src/Infrastructure/`)
**Purpose**: Implements application interfaces for external concerns (database, email, file system, identity, etc.)
**Dependencies**: `Application` layer, `Domain` layer

#### Key Principles:
- **EF Core Configuration**:
  - Fluent API in `Configurations/` folder
  - Interceptors for audit trails, domain events, multi-tenancy
  - Support for MSSQL, PostgreSQL, SQLite
  
- **Service Implementations**:
  - Identity services (authentication, authorization, user management)
  - Email services (FluentEmail with MailKit)
  - File upload services
  - Excel/PDF export services
  - Caching (FusionCache with Redis support)
  
- **Multi-Tenancy**: Tenant isolation at database level

#### Structure:
```
Infrastructure/
├── Persistence/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/    # EF Core entity configurations
│   ├── Interceptors/      # EF Core interceptors
│   └── Migrations/
├── Services/              # Service implementations
│   ├── Identity/
│   ├── Email/
│   ├── FileUpload/
│   └── MultiTenant/
├── PermissionSet/         # Permission definitions
└── DependencyInjection.cs
```

**Rules**:
- ✅ Implement all Application layer interfaces
- ✅ Use EF Core Fluent API for entity configuration
- ✅ Implement interceptors for cross-cutting concerns
- ✅ Register all services in `DependencyInjection.cs`
- ✅ Use migration projects for database-specific migrations
- ❌ NO business logic (delegate to Application layer)
- ❌ NO direct entity manipulation in services (use DbContext)

---

### 4. **Presentation Layer** (`src/Server.UI/`)
**Purpose**: Blazor Server UI, components, pages, and user interaction
**Dependencies**: `Application` layer, `Infrastructure` layer

#### Key Principles:
- **MudBlazor** for primary UI components and theming
- **Telerik UI for Blazor** for advanced data-intensive components
- **SignalR** for real-time updates
- **Blazor Components**: Organized by feature
- **Services**: UI-specific services (navigation, notifications, layout)
- **Responsive Design**: All components must be mobile-friendly
- **Global Theming**: Automatic theme synchronization handled in MainLayout (light/dark mode applies to both MudBlazor and Telerik)

#### Structure:
```
Server.UI/
├── Components/            # Reusable Blazor components
├── Pages/                 # Blazor pages/routes
│   ├── Products/
│   ├── Contacts/
│   └── Authentication/
├── Services/              # UI services
├── Hubs/                  # SignalR hubs
├── Middlewares/           # ASP.NET middleware
└── Program.cs
```

**Rules**:
- ✅ Send commands/queries via MediatR
- ✅ Work with DTOs, never domain entities
- ✅ Use dependency injection
- ✅ Handle errors gracefully with user feedback
- ✅ Use Telerik components for data grids, charts, and complex data visualization
- ✅ Maintain responsive design across all screen sizes
- ✅ Keep UI consistent with established theme
- ❌ NO direct database access
- ❌ NO business logic in components

---

## UI Component Strategy: MudBlazor vs Telerik

### Component Selection Guidelines

This project integrates both **MudBlazor** (primary) and **Telerik UI for Blazor** (specialized) to leverage the strengths of each library.

#### When to Use MudBlazor 🎨
Use MudBlazor for general UI components and layouts:
- ✅ **Layout Components**: AppBar, Drawer, Container, Paper
- ✅ **Navigation**: Tabs, Breadcrumbs, Menu, NavLink
- ✅ **Forms**: TextField, Select, Checkbox, Radio, Switch, DatePicker
- ✅ **Buttons**: Button, IconButton, Fab, ButtonGroup
- ✅ **Dialogs**: Dialog, Snackbar, Alert
- ✅ **Cards & Lists**: Card, List, ExpansionPanel
- ✅ **Simple Tables**: MudTable for basic CRUD with <50 rows
- ✅ **Charts**: Simple charts and visualizations
- ✅ **Progress**: Spinner, ProgressLinear, ProgressCircular

**Reasoning**: MudBlazor provides the base theme and design system. Use it for consistency.

#### When to Use Telerik 📊
Use Telerik for advanced, data-intensive, and complex interactive components:
- ✅ **Data Grids**: TelerikGrid for complex tables with:
  - Large datasets (>100 rows)
  - Advanced filtering (filter row, filter menu)
  - Grouping and aggregates
  - Hierarchical data
  - Virtual scrolling
  - Excel-like editing
  - Complex cell templates
  
- ✅ **Charts & Visualizations**: TelerikChart for:
  - Interactive dashboards
  - Multiple chart types (Line, Bar, Pie, Donut, etc.)
  - Real-time data updates
  - Drill-down capabilities
  
- ✅ **Scheduler**: TelerikScheduler for:
  - Calendar views
  - Appointment management
  - Resource scheduling
  
- ✅ **TreeView/TreeList**: Hierarchical data display
- ✅ **Gantt Chart**: Project management timelines
- ✅ **Spreadsheet**: Excel-like data editing
- ✅ **PDF Viewer**: Document viewing and annotation
- ✅ **File Upload**: Advanced file upload with progress
- ✅ **Data Entry**: When you need:
  - AutoComplete with templates
  - Multi-select with grouping
  - Numeric TextBox with formatting
  - Rich Text Editor

**Reasoning**: Telerik excels at enterprise-grade data components with advanced features.

### Telerik Integration Setup

**Note**: Telerik is already configured and integrated in this project. Theming (light/dark mode) is handled globally in MainLayout.razor and automatically applies to all Telerik components.

#### 1. Component Registration (Already Configured)
```csharp
// src/Server.UI/DependencyInjection.cs
services.AddTelerikBlazor();
```

#### 2. Global Imports (Already Configured)
```razor
@* src/Server.UI/_Imports.razor *@
@using Telerik.Blazor
@using Telerik.Blazor.Components
```

#### 3. Namespace Conflict Resolution (Already Configured)
```razor
@* In _Imports.razor *@
@using TelerikGridState = Telerik.Blazor.Components.GridState
@using MudGridState = MudBlazor.State
```

#### 4. Global Theme Management (Already Implemented)
Theme switching is handled automatically in MainLayout.razor:
- Light mode: Uses Telerik's default theme
- Dark mode: Uses custom dark theme from `wwwroot/css/dark/dist/css/dark.css`
- Synchronized with MudBlazor theme changes
- No manual theme configuration needed in individual components

### Telerik Grid Example (Complete)

```razor
@page "/products/telerik"
@inject IMediator Mediator

<PageTitle>Product Management</PageTitle>

<MudContainer MaxWidth="MaxWidth.ExtraExtraLarge" Class="mt-4">
    <MudPaper Elevation="2" Class="pa-4">
        <div class="d-flex justify-space-between mb-4">
            <div>
                <MudText Typo="Typo.h5">Product Catalog</MudText>
                <MudText Typo="Typo.body2" Color="Color.Secondary">
                    Advanced grid with filtering and sorting
                </MudText>
            </div>
            <MudButton Variant="Variant.Filled" 
                       Color="Color.Primary" 
                       OnClick="@OpenCreateDialog">
                <MudIcon Icon="@Icons.Material.Filled.Add" Class="mr-2" />
                Add Product
            </MudButton>
        </div>

        <div class="telerik-grid-wrapper">
            <TelerikGrid Data="@Products"
                         Pageable="true"
                         PageSize="15"
                         Sortable="true"
                         FilterMode="GridFilterMode.FilterRow"
                         Resizable="true"
                         Reorderable="true"
                         SelectionMode="GridSelectionMode.Multiple"
                         @bind-SelectedItems="@SelectedProducts"
                         OnUpdate="@UpdateHandler"
                         OnDelete="@DeleteHandler"
                         Height="600px">
                <GridToolBarTemplate>
                    <GridCommandButton Command="Add" Icon="@SvgIcon.Plus">Add</GridCommandButton>
                    <GridCommandButton OnClick="@RefreshData" Icon="@SvgIcon.Reload">Refresh</GridCommandButton>
                    <GridCommandButton OnClick="@ExportToExcel" Icon="@SvgIcon.FileExcel">Export</GridCommandButton>
                </GridToolBarTemplate>
                <GridColumns>
                    <GridColumn Field="@nameof(ProductDto.Id)" 
                                Title="ID" 
                                Width="80px" 
                                Editable="false" />
                    <GridColumn Field="@nameof(ProductDto.Name)" 
                                Title="Product Name" />
                    <GridColumn Field="@nameof(ProductDto.Category)" 
                                Title="Category">
                        <FilterCellTemplate>
                            <TelerikDropDownList Data="@Categories"
                                                @bind-Value="@context.FilterContext.Value"
                                                DefaultText="All Categories" />
                        </FilterCellTemplate>
                    </GridColumn>
                    <GridColumn Field="@nameof(ProductDto.Price)" 
                                Title="Price" 
                                DisplayFormat="{0:C2}" />
                    <GridColumn Field="@nameof(ProductDto.Stock)" 
                                Title="Stock" 
                                Width="100px" />
                    <GridColumn Field="@nameof(ProductDto.IsActive)" 
                                Title="Status" 
                                Width="120px">
                        <Template>
                            @{
                                var product = context as ProductDto;
                                <MudChip Size="Size.Small" 
                                         Color="@(product.IsActive ? Color.Success : Color.Default)">
                                    @(product.IsActive ? "Active" : "Inactive")
                                </MudChip>
                            }
                        </Template>
                    </GridColumn>
                    <GridCommandColumn Width="180px">
                        <GridCommandButton Command="Edit" Icon="@SvgIcon.Pencil">Edit</GridCommandButton>
                        <GridCommandButton Command="Delete" Icon="@SvgIcon.Trash">Delete</GridCommandButton>
                    </GridCommandColumn>
                </GridColumns>
            </TelerikGrid>
        </div>
    </MudPaper>
</MudContainer>

@code {
    private List<ProductDto> Products { get; set; } = new();
    private IEnumerable<ProductDto> SelectedProducts { get; set; } = new List<ProductDto>();
    private List<string> Categories { get; set; } = new() { "Electronics", "Clothing", "Food" };

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        var query = new GetAllProductsQuery();
        var result = await Mediator.Send(query);
        Products = result.ToList();
    }

    private async Task UpdateHandler(GridCommandEventArgs args)
    {
        var product = args.Item as ProductDto;
        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
        await Mediator.Send(command);
        await LoadData();
    }

    private async Task DeleteHandler(GridCommandEventArgs args)
    {
        var product = args.Item as ProductDto;
        var command = new DeleteProductCommand { Id = product.Id };
        await Mediator.Send(command);
        await LoadData();
    }

    private async Task RefreshData()
    {
        await LoadData();
    }

    private async Task ExportToExcel()
    {
        // Implement export logic
    }
}
```

### Global Theme Management

**Important**: Theming is handled globally in `MainLayout.razor`. When the user switches between light and dark mode, both MudBlazor and Telerik components update automatically.

```csharp
// src/Server.UI/Components/Shared/Layout/MainLayout.razor
private async Task UpdateTelerikTheme()
{
    var isDark = LayoutService.IsDarkMode;
    var themePath = isDark 
        ? "css/dark/dist/css/dark.css"
        : "_content/Telerik.UI.for.Blazor/css/kendo-theme-default/all.css";
    
    await JS.InvokeVoidAsync("changeTelerikTheme", themePath);
}
```

**No manual theming required** in individual components - just use Telerik components and they will automatically match the current theme.

### Responsive Design Patterns

#### Grid Responsiveness
```razor
<div class="responsive-grid-container">
    <TelerikGrid Data="@Data"
                 Height="auto"
                 Class="responsive-grid">
        <GridColumns>
            @* Always visible columns *@
            <GridColumn Field="@nameof(Model.Id)" Width="80px" />
            <GridColumn Field="@nameof(Model.Name)" MinResizableWidth="120" />
            
            @* Hidden on mobile *@
            <GridColumn Field="@nameof(Model.Description)" 
                        HeaderClass="hide-on-mobile"
                        Class="hide-on-mobile" />
            
            @* Action column *@
            <GridCommandColumn Width="150px" />
        </GridColumns>
    </TelerikGrid>
</div>

<style>
    @@media (max-width: 768px) {
        .hide-on-mobile {
            display: none !important;
        }
        
        .responsive-grid-container {
            overflow-x: auto;
        }
    }
</style>
```

#### Mobile-First Card View Alternative
```razor
@if (IsMobile)
{
    @* Card view for mobile *@
    <MudGrid>
        @foreach (var item in Data)
        {
            <MudItem xs="12" sm="6" md="4">
                <MudCard>
                    <MudCardHeader>
                        <MudText Typo="Typo.h6">@item.Name</MudText>
                    </MudCardHeader>
                    <MudCardContent>
                        <MudText>@item.Description</MudText>
                        <MudText Typo="Typo.body2">Price: @item.Price.ToString("C")</MudText>
                    </MudCardContent>
                    <MudCardActions>
                        <MudIconButton Icon="@Icons.Material.Filled.Edit" 
                                       OnClick="@(() => Edit(item))" />
                        <MudIconButton Icon="@Icons.Material.Filled.Delete" 
                                       OnClick="@(() => Delete(item))" />
                    </MudCardActions>
                </MudCard>
            </MudItem>
        }
    </MudGrid>
}
else
{
    @* Telerik Grid for desktop *@
    <TelerikGrid Data="@Data" ...>
        ...
    </TelerikGrid>
}

@code {
    [Inject] private IJSRuntime JS { get; set; }
    private bool IsMobile { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            IsMobile = await JS.InvokeAsync<bool>("matchMedia", "(max-width: 768px)");
            StateHasChanged();
        }
    }
}
```

### Telerik Component Checklist

When implementing Telerik components, ensure:
- [ ] **Theming**: Component matches MudBlazor design system
- [ ] **Responsive**: Works on mobile, tablet, and desktop
- [ ] **Accessibility**: ARIA labels, keyboard navigation
- [ ] **Performance**: Virtual scrolling for large datasets
- [ ] **Error Handling**: User-friendly error messages
- [ ] **Loading States**: Show progress indicators
- [ ] **Data Binding**: Use DTOs from Application layer via MediatR
- [ ] **Localization**: Support multiple languages
- [ ] **Export**: Implement Excel/PDF export where applicable
- [ ] **State Management**: Persist grid state (filters, sorting, pagination)

### Common Telerik Patterns

#### Grid with Server-Side Operations
```csharp
// Use OnRead event for server-side paging, sorting, filtering
<TelerikGrid TItem="ProductDto"
             OnRead="@ReadItems"
             Pageable="true"
             Sortable="true"
             FilterMode="GridFilterMode.FilterRow">
    ...
</TelerikGrid>

@code {
    private async Task ReadItems(GridReadEventArgs args)
    {
        var query = new GetProductsWithPaginationQuery
        {
            PageNumber = args.Request.Page,
            PageSize = args.Request.PageSize,
            OrderBy = string.Join(",", args.Request.Sorts.Select(s => s.Member)),
            // Map Telerik filters to Application layer filters
        };
        
        var result = await Mediator.Send(query);
        args.Data = result.Items;
        args.Total = result.TotalCount;
    }
}
```

#### ChaResponsive**: Works on mobile, tablet, and desktop
- [ ] **Accessibility**: ARIA labels, keyboard navigation
- [ ] **Performance**: Virtual scrolling for large datasets
- [ ] **Error Handling**: User-friendly error messages
- [ ] **Loading States**: Show progress indicators
- [ ] **Data Binding**: Use DTOs from Application layer via MediatR
- [ ] **Localization**: Support multiple languages
- [ ] **Export**: Implement Excel/PDF export where applicable
- [ ] **State Management**: Persist grid state (filters, sorting, pagination)

**Note**: Theming is handled globally - no manual theme configuration needed.
</TelerikChart>

@code {
    private Timer _timer;
    private List<DataPoint> ChartData { get; set; } = new();

    protected override void OnInitialized()
    {
        _timer = new Timer(async _ => await UpdateChart(), null, 0, 5000);
    }

    private async Task UpdateChart()
    {
        var query = new GetRealtimeDataQuery();
        var result = await Mediator.Send(query);
        ChartData = result.ToList();
        await InvokeAsync(StateHasChanged);
    }
}
```

---

## Development Patterns & Guidelines

### 1. **Adding a New Feature**

#### Step 1: Create Domain Entity

**Option A: Int Primary Key (Default)**
```csharp
// src/Domain/Entities/Customer.cs
public class Customer : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}
```

**Option B: String Primary Key**
```csharp
// src/Domain/Entities/Order.cs
public class Order : BaseAuditableEntityWithStringKey
{
    public Order()
    {
        // Generate string-based ID (GUID, custom format, etc.)
        Id = Guid.NewGuid().ToString();
        // Or: Id = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{GenerateSequence():D5}";
    }
    
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }

**For Int Primary Key:**
```csharp
// src/Infrastructure/Persistence/Configurations/CustomerConfiguration.cs
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
```

**For String Primary Key:**
```csharp
// src/Infrastructure/Persistence/Configurations/OrderConfiguration.cs
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // IMPORTANT: Always specify MaxLength for string primary keys
        builder.Property(x => x.Id).HasMaxLength(50).IsRequired();
        
        builder.Property(x => x.OrderNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.OrderNumberset; }
    // ... other DbSets
}
```

#### Step 3: Implement DbSet in ApplicationDbContext
```csharp
// src/Infrastructure/Persistence/ApplicationDbContext.cs
public DbSet<Customer> Customers => Set<Customer>();
```

#### Step 4: Create EF Core Configuration
```csharp
// src/Infrastructure/Persistence/Configurations/CustomerConfiguration.cs
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
```

#### Step 5: Create Command/Query Structure
```
Application/Features/Customers/
├── Commands/
│   ├── AddEdit/
│   │   ├── AddEditCustomerCommand.cs
│   │   ├── AddEditCustomerCommandValidator.cs
│   └── Delete/
│       └── DeleteCustomerCommand.cs
├── Queries/
│   ├── GetAll/
│   └── Pagination/
├── DTOs/
│   └── CustomerDto.cs
├── Caching/
│   └── CustomerCacheKey.cs
└── Specifications/
    └── CustomerAdvancedSpecification.cs
```

#### Step 6: Implement Command with Validation
```csharp
// AddEditCustomerCommand.cs
public class AddEditCustomerCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => CustomerCacheKey.Tags;
}

// AddEditCustomerCommandValidator.cs
public class AddEditCustomerCommandValidator : AbstractValidator<AddEditCustomerCommand>
{
    public AddEditCustomerCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.Email).MaximumLength(256).EmailAddress().NotEmpty();
    }
}

// AddEditCustomerCommandHandler.cs
public class AddEditCustomerCommandHandler : IRequestHandler<AddEditCustomerCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public AddEditCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(AddEditCustomerCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Customers.FindAsync(request.Id);
            if (item == null) return await Result<int>.FailureAsync($"Customer not found.");
            
            item.Name = request.Name;
            item.Email = request.Email;
            item.AddDomainEvent(new UpdatedEvent<Customer>(item));
        }
        else
        {
            var item = new Customer { Name = request.Name, Email = request.Email };
            item.AddDomainEvent(new CreatedEvent<Customer>(item));
            _context.Customers.Add(item);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}
```

### 2. **Query Pattern with Specifications**
```csharp
// CustomerAdvancedSpecification.cs
public class CustomerAdvancedSpecification : Specification<Customer>
{
    public CustomerAdvancedSpecification(CustomerAdvancedFilter filter)
    {
        Query.Where(x => x.Name.Contains(filter.Keyword) || 
                         x.Email.Contains(filter.Keyword), 
                    !string.IsNullOrEmpty(filter.Keyword));
    }
}

// GetCustomersQuery.cs
public class GetCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
    public string? Keyword { get; set; }
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly IApplicationDbContext _context;

    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var filter = new CustomerAdvancedFilter { Keyword = request.Keyword };
        var spec = new CustomerAdvancedSpecification(filter);
        
        return await _context.Customers
            .WithSpecification(spec)
            .Select(x => new CustomerDto 
            { 
                Id = x.Id, 
                Name = x.Name, 
                Email = x.Email 
            })
            .ToListAsync(cancellationToken);
    }
}
```

### 3. **Domain Event Handling**
```csharp
// Domain Event
public class CustomerCreatedEvent : DomainEvent
{
    public CustomerCreatedEvent(Customer customer)
    {
        Customer = customer;
    }
    
    public Customer Customer { get; }
}

// Event Handler
public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
{
    private readonly IMailService _mailService;

    public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Send welcome email
        await _mailService.SendAsync(notification.Customer.Email, "Welcome", "...");
    }
}
```

---

## Critical Rules & Constraints

### Dependency Rules
1. **Domain** → No dependencies
2. **Application** → Domain only
3. **Infrastructure** → Application + Domain
4. **Presentation** → Application + Infrastructure

### Never Do This ❌
- **Don't bypass MediatR**: Never call handlers directly
- **Don't expose entities**: Always use DTOs in responses
- **Don't put business logic in UI**: Keep it in Application layer
- **Don't put business logic in Infrastructure**: It belongs in Application
- **Don't reference Infrastructure from Application**: Use interfaces
- **Don't use `DbContext` directly**: Use `IApplicationDbContext`
- **Don't create circular dependencies**: Follow the layer hierarchy

### Always Do This ✅
- **Use MediatR** for all business operations
- **Validate commands** with FluentValidation
- **Use DTOs** for data transfer
- **Define interfaces** in Application, implement in Infrastructure
- **Raise domain events** for important state changes
- **Use specifications** for complex queries
- **Implement caching** for frequently accessed data
- **Follow naming conventions**: `{Action}{Entity}Command/Query`
- **One handler per command/query**: Keep handlers focused
- **Use dependency injection**: Register services properly

---

## Testing Strategy

### Test Projects Structure
```
tests/
├── Domain.UnitTests/         # Domain logic tests
├── Application.UnitTests/    # Command/Query handler tests
└── Application.IntegrationTests/  # Database integration tests
```

### Testing Guidelines
- **Unit Tests**: Test domain logic, validators, mappers
- **Integration Tests**: Test database operations, full request pipeline
- **Use**: xUnit, FluentAssertions, Moq
- **Mock**: External dependencies (IMailService, IFileService, etc.)
- **Test**: Validation rules, business logic, error scenarios

---

## Code Generation & Scaffolding

This project uses Visual Studio extensions for code generation. When adding features:
1. Use consistent folder structure (Commands/Queries/DTOs/Specifications)
2. Follow established naming patterns
3. Implement all necessary validators
4. Add cache invalidation
5. Create specifications for filtering

---

## Multi-Tenancy Considerations

- All tenant-aware entities implement `IMayHaveTenant`
- Tenant filtering is automatic via EF Core interceptors
- Current tenant resolved from user claims
- Use `ITenantService` for tenant operations

---

## Security & Authorization

- Claims-based authorization
- Permission-based access control defined in `PermissionSet`
- Use `[Authorize]` attributes on pages/commands
- Implement `IAuthorizationHandler` for custom policies
- Multi-factor authentication support

---

## Performance Optimization

- **Caching**: Use `FusionCacheBehaviour` for query results
- **Cache Invalidation**: Implement `ICacheInvalidatorRequest` on commands
- **Pagination**: Always use pagination for large datasets
- **Projections**: Select only needed fields with `.Select()`
- **Monitoring**: `PerformanceBehaviour` logs slow requests (>500ms)
- **Background Jobs**: Use Hangfire for async processing

---

## Migration Workflow

1. Make changes to domain entities
2. Add/update EF Core configurations
3. Create migration: `dotnet ef migrations add MigrationName -p src/Migrators/Migrators.MSSQL`
4. Review generated migration
5. Apply: `dotnet ef database update`

---

## Summary Checklist for New Features

- [ ] Create domain entity in `Domain/Entities/`
- [ ] Add DbSet to `IApplicationDbContext` and `ApplicationDbContext`
- [ ] Create EF Core configuration in `Infrastructure/Persistence/Configurations/`
- [ ] Create feature folder in `Application/Features/`
- [ ] Implement commands with validators
- [ ] Implement queries with specifications
- [ ] Create DTOs
- [ ] Implement cache keys and invalidation
- [ ] Add domain event handlers if needed
- [ ] Create Blazor components/pages in `Server.UI/`
- [ ] **Choose appropriate UI components**:
  - [ ] Use MudBlazor for forms, buttons, dialogs, navigation
  - [ ] Use Telerik for data grids (>50 rows), charts, complex data entry
- [ ] **Ensure responsive design**:
  - [ ] Test on mobile (< 768px)
  - [ ] Test on tablet (768px - 1024px)
  - [ Add unit and integration tests
- [ ] Update documentation

**Note**: Theme consistency is handled globally - both MudBlazor and Telerik automatically sync with light/dark mode.tion tests
- [ ] Update documentation

---

## AI Assistant Instructions

When helping with this project:

1. **Respect layer boundaries**: Never suggest code that violates dependency rules
2. **Follow patterns**: Use existing code as templates for new features
3. **Complete implementations**: Include validators, DTOs, specifications
4. **Maintain consistency**: Follow naming conventions and folder structure
5. **Consider cross-cutting concerns**: Caching, validation, logging, events
6. **Security first**: Apply authorization, validate input, sanitize output
7. **Think testability**: Write code that's easy to unit test
8. **Document decisions**: Add XML comments for public APIs

When generating code:
- Always show the full file path
- Include necessary using statements
- Follow C# coding standards (PascalCase for public members, etc.)
- Use nullable reference types appropriately
- Implement async/await properly with CancellationToken
- Handle exceptions gracefully with meaningful messages
- **UI Component Selection**:
  - Default to MudBlazor for standard UI elements
  - Recommend Telerik for data-intensive scenarios
  - Explain trade-offs when multiple options exist
- **Responsive Design**:
  - Always include responsive CSS classes
  - Test layouts at different breakpoints
  - Suggest mobile-friendly alternatives when needed
- **Theming**:
  - Maintain color consistency with MudBlazor palette
  - Theme management is global - no manual configuration needed
  - Both MudBlazor and Telerik sync automatically with light/dark mode
  - Custom styling should use CSS classes, not theme overrides
---

**Last Updated**: February 5, 2026  
**Project Version**: .NET 10  
**Template**: CleanArchitecture.Blazor.Solution.Template
