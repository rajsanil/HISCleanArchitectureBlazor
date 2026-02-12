using System.Reflection;

namespace HIS.Core.Abstractions;

/// <summary>
/// Service responsible for discovering, validating, and loading HIS modules.
/// </summary>
public interface IModuleLoader
{
    /// <summary>
    /// Discovers all available modules from referenced assemblies.
    /// </summary>
    IEnumerable<IHisModule> DiscoverModules();

    /// <summary>
    /// Gets all modules that are both available and licensed.
    /// Validates dependencies and returns modules in dependency order.
    /// </summary>
    IEnumerable<IHisModule> GetActiveModules();

    /// <summary>
    /// Gets a specific module by its ID.
    /// </summary>
    IHisModule? GetModule(string moduleId);

    /// <summary>
    /// Validates that all module dependencies are satisfied.
    /// Throws exception if circular dependencies or missing dependencies are found.
    /// </summary>
    /// <param name="modules">Modules to validate</param>
    void ValidateDependencies(IEnumerable<IHisModule> modules);

    /// <summary>
    /// Returns modules sorted in dependency order (dependencies before dependents).
    /// </summary>
    IEnumerable<IHisModule> SortByDependencies(IEnumerable<IHisModule> modules);

    /// <summary>
    /// Gets the assemblies that should be scanned for MediatR handlers and validators.
    /// </summary>
    IEnumerable<Assembly> GetApplicationAssemblies();

    /// <summary>
    /// Gets the assemblies that should be scanned for EF Core configurations.
    /// </summary>
    IEnumerable<Assembly> GetInfrastructureAssemblies();

    /// <summary>
    /// Gets all UI assemblies for Razor component routing.
    /// </summary>
    IEnumerable<Assembly> GetUIAssemblies();
}
