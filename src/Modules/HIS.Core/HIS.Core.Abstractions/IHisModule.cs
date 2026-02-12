using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Core.Abstractions;

/// <summary>
/// Defines the contract for HIS modules that can be dynamically loaded and composed.
/// Each module implements this interface to self-register its services, entities, and UI components.
/// </summary>
public interface IHisModule
{
    /// <summary>
    /// Unique identifier for the module (e.g., "HIS.MasterData", "HIS.Patient")
    /// Used for dependency tracking and license validation.
    /// </summary>
    string ModuleId { get; }

    /// <summary>
    /// Display name shown in admin UI and logs (e.g., "Master Data Management")
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Semantic version of the module (e.g., "1.0.0")
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Array of module IDs that this module depends on.
    /// The module loader will ensure dependencies are activated before this module.
    /// </summary>
    string[] Dependencies { get; }

    /// <summary>
    /// Optional: Minimum version requirements for dependencies.
    /// Key: ModuleId, Value: Minimum version (e.g., ["HIS.Foundation"] = "1.2.0")
    /// </summary>
    IDictionary<string, string>? MinimumDependencyVersions => null;

    /// <summary>
    /// Configure domain-level services (if any).
    /// Typically modules don't need domain-level DI, but this is available for extensibility.
    /// </summary>
    void ConfigureDomainServices(IServiceCollection services) { }

    /// <summary>
    /// Configure application-level services (MediatR handlers, validators, etc.)
    /// The module loader will scan the Application assembly for handlers automatically.
    /// Use this for manual registrations if needed.
    /// </summary>
    void ConfigureApplicationServices(IServiceCollection services) { }

    /// <summary>
    /// Configure infrastructure services (repositories, external service clients, etc.)
    /// Register module-specific implementations of interfaces here.
    /// </summary>
    void ConfigureInfrastructureServices(IServiceCollection services, IConfiguration configuration) { }

    /// <summary>
    /// Configure UI services (dialog services, state management, etc.)
    /// </summary>
    void ConfigureUIServices(IServiceCollection services) { }

    /// <summary>
    /// Configure EF Core entity mappings and configurations.
    /// Called during ApplicationDbContext.OnModelCreating().
    /// Typically calls builder.ApplyConfigurationsFromAssembly() for the module's Infrastructure assembly.
    /// </summary>
    void ConfigureDatabase(ModelBuilder modelBuilder);

    /// <summary>
    /// Returns menu sections/items to be displayed in the navigation menu.
    /// The host will merge these with the core platform menus and filter by permissions.
    /// </summary>
    IEnumerable<ModuleMenuSection> GetMenuSections();

    /// <summary>
    /// Returns permission types defined by this module.
    /// The host will use reflection to discover permission constants and register them as policies.
    /// </summary>
    IEnumerable<Type> GetPermissionTypes();

    /// <summary>
    /// Returns the assemblies containing Razor components for this module.
    /// Used to register additional assemblies for routing.
    /// </summary>
    IEnumerable<System.Reflection.Assembly> GetUIAssemblies() => [];
}
