# HIS Clean Architecture Blazor

A Hospital Information System (HIS) built with Clean Architecture principles using Blazor Server and .NET 10.

## Project Structure

This solution follows Clean Architecture principles, organizing the codebase into distinct layers with clear dependency rules:

```
HIS/
├── src/
│   ├── Domain/              # Core business logic (No dependencies)
│   │   └── HIS.Domain/
│   │       ├── Common/      # Base entities and shared types
│   │       ├── Entities/    # Domain entities (Patient, Appointment)
│   │       └── Interfaces/  # Domain interfaces (IRepository)
│   │
│   ├── Application/         # Business logic and use cases (Depends on Domain)
│   │   └── HIS.Application/
│   │       ├── DTOs/        # Data Transfer Objects
│   │       ├── Interfaces/  # Service interfaces
│   │       └── Services/    # Business logic implementation
│   │
│   ├── Infrastructure/      # Data access and external services (Depends on Domain & Application)
│   │   └── HIS.Infrastructure/
│   │       ├── Data/        # DbContext
│   │       └── Repositories/ # Repository implementations
│   │
│   └── Presentation/        # User interface (Depends on Application & Infrastructure)
│       └── HIS.Blazor/
│           ├── Components/  # Blazor components
│           │   ├── Layout/  # Layout components
│           │   └── Pages/   # Page components
│           └── wwwroot/     # Static assets
│
└── tests/                   # Test projects (to be added)

```

## Architecture Layers

### 1. Domain Layer (HIS.Domain)
The innermost layer containing:
- **Entities**: Core business objects (Patient, Appointment)
- **Common**: Base classes and shared types
- **Interfaces**: Domain contracts (IRepository)
- **No external dependencies** - ensures domain logic remains independent

### 2. Application Layer (HIS.Application)
Contains application-specific business rules:
- **DTOs**: Data Transfer Objects for cross-layer communication
- **Interfaces**: Service contracts (IPatientService, IAppointmentService)
- **Services**: Business logic implementation
- **Depends only on Domain layer**

### 3. Infrastructure Layer (HIS.Infrastructure)
Handles external concerns:
- **Data**: Entity Framework Core DbContext
- **Repositories**: Generic repository implementation
- **Dependency Injection**: Service registration
- **Uses Entity Framework Core InMemory database** (can be replaced with SQL Server, PostgreSQL, etc.)

### 4. Presentation Layer (HIS.Blazor)
Blazor Server application providing the UI:
- **Pages**: Patient management, Appointment scheduling
- **Components**: Reusable UI components
- **Interactive Server rendering mode**

## Features

### Patient Management
- Create, Read, Update, Delete (CRUD) operations for patients
- Medical Record Number (MRN) auto-generation
- Patient demographics (name, DOB, gender, contact info)

### Appointment Scheduling
- Schedule appointments with doctors
- Link appointments to patients
- Track appointment status
- View upcoming appointments

## Technology Stack

- **.NET 10**
- **Blazor Server** - Interactive web UI
- **Entity Framework Core 10** - ORM
- **In-Memory Database** - For demonstration (easily replaceable)
- **Bootstrap 5** - UI styling

## Getting Started

### Prerequisites
- .NET 10 SDK or later

### Running the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/rajsanil/HISCleanArchitectureBlazor.git
   cd HISCleanArchitectureBlazor
   ```

2. Build the solution:
   ```bash
   dotnet build
   ```

3. Run the Blazor application:
   ```bash
   cd src/Presentation/HIS.Blazor
   dotnet run
   ```

4. Open your browser and navigate to `https://localhost:5001` (or the URL shown in the console)

## Clean Architecture Benefits

1. **Independence**: Domain logic is independent of frameworks, UI, and databases
2. **Testability**: Each layer can be tested independently
3. **Maintainability**: Clear separation of concerns makes the code easier to maintain
4. **Flexibility**: Easy to swap implementations (e.g., change database from InMemory to SQL Server)
5. **Scalability**: Well-organized structure supports growth

## Dependency Flow

```
Presentation → Application → Domain
                           ↑
Infrastructure ────────────┘
```

- **Domain** has no dependencies
- **Application** depends only on Domain
- **Infrastructure** depends on Domain and Application (implements interfaces)
- **Presentation** depends on Application and Infrastructure (for DI)

## Future Enhancements

- Add authentication and authorization
- Implement API layer for external integrations
- Add comprehensive unit and integration tests
- Implement real database (SQL Server, PostgreSQL)
- Add more entities (Doctor, Department, MedicalRecord, etc.)
- Implement advanced features (billing, prescriptions, lab results)
- Add validation using FluentValidation
- Implement CQRS pattern with MediatR
- Add logging and monitoring

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.
