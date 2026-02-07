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
  - `BaseEntity`: Abstract base with `Id` and domain events collection
  - `BaseAuditableEntity`: Extends `BaseEntity` with audit fields (Created, CreatedBy, LastModified, LastModifiedBy)
  - `BaseAuditableSoftDeleteEntity`: Adds soft delete capability with `Deleted` timestamp
  
- **Domain Events**: All entities can raise domain events via `AddDomainEvent()`, `RemoveDomainEvent()`, `ClearDomainEvents()`
  - Built-in events: `CreatedEvent<T>`, `UpdatedEvent<T>`, `DeletedEvent<T>`

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

---

## Development Patterns & Guidelines

### 1. **Adding a New Feature**

#### Step 1: Create Domain Entity
```csharp
// src/Domain/Entities/Customer.cs
public class Customer : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}
```

#### Step 2: Add DbSet to IApplicationDbContext
```csharp
// src/Application/Common/Interfaces/IApplicationDbContext.cs
public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; set; }
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
- [ ] Add unit and integration tests
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
