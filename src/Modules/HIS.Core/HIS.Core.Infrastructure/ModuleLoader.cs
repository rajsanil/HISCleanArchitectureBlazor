using System.Reflection;
using HIS.Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace HIS.Core.Infrastructure;

/// <summary>
/// Service responsible for discovering, validating, and loading HIS modules.
/// </summary>
public class ModuleLoader : IModuleLoader
{
    private readonly ILicenseService _licenseService;
    private readonly ILogger<ModuleLoader> _logger;
    private List<IHisModule>? _discoveredModules;
    private List<IHisModule>? _activeModules;
    private readonly object _lock = new();

    public ModuleLoader(ILicenseService licenseService, ILogger<ModuleLoader> logger)
    {
        _licenseService = licenseService;
        _logger = logger;
    }

    public IEnumerable<IHisModule> DiscoverModules()
    {
        lock (_lock)
        {
            if (_discoveredModules != null)
            {
                return _discoveredModules;
            }

            _logger.LogInformation("Discovering HIS modules...");
            _discoveredModules = [];

            try
            {
                // Force load module assemblies from the application's bin directory
                LoadModuleAssemblies();
                
                // Get all loaded assemblies
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic && a.GetName().Name?.StartsWith("HIS.") == true)
                    .ToList();

                _logger.LogInformation("Found {Count} HIS assemblies: {Assemblies}", 
                    assemblies.Count, 
                    string.Join(", ", assemblies.Select(a => a.GetName().Name)));

                foreach (var assembly in assemblies)
                {
                    try
                    {
                        // Find types implementing IHisModule
                        var moduleTypes = assembly.GetTypes()
                            .Where(t => typeof(IHisModule).IsAssignableFrom(t) 
                                     && !t.IsInterface 
                                     && !t.IsAbstract);

                        foreach (var moduleType in moduleTypes)
                        {
                            try
                            {
                                var module = (IHisModule?)Activator.CreateInstance(moduleType);
                                if (module != null)
                                {
                                    _discoveredModules.Add(module);
                                    _logger.LogInformation("Discovered module: {ModuleId} v{Version} from {Assembly}",
                                        module.ModuleId, module.Version, assembly.GetName().Name);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error creating instance of module type: {TypeName}", moduleType.FullName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error scanning assembly: {AssemblyName}", assembly.GetName().Name);
                    }
                }

                _logger.LogInformation("Discovered {Count} modules total", _discoveredModules.Count);
                return _discoveredModules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error discovering modules");
                return _discoveredModules;
            }
        }
    }

    public IEnumerable<IHisModule> GetActiveModules()
    {
        lock (_lock)
        {
            if (_activeModules != null)
            {
                return _activeModules;
            }

            var discovered = DiscoverModules();
            var licensed = discovered.Where(m => _licenseService.IsModuleLicensed(m.ModuleId)).ToList();

            _logger.LogInformation("Filtering {DiscoveredCount} discovered modules by license. Licensed: {LicensedCount}",
                discovered.Count(), licensed.Count);

            // Validate dependencies
            try
            {
                ValidateDependencies(licensed);
            }
            catch (ModuleException ex)
            {
                _logger.LogError(ex, "Module dependency validation failed");
                throw;
            }

            // Sort by dependencies
            _activeModules = SortByDependencies(licensed).ToList();

            _logger.LogInformation("Active modules ({Count}): {Modules}",
                _activeModules.Count,
                string.Join(", ", _activeModules.Select(m => m.ModuleId)));

            return _activeModules;
        }
    }

    public IHisModule? GetModule(string moduleId)
    {
        return GetActiveModules().FirstOrDefault(m =>
            m.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase));
    }

    public void ValidateDependencies(IEnumerable<IHisModule> modules)
    {
        var moduleList = modules.ToList();
        var moduleIds = moduleList.Select(m => m.ModuleId).ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Check for missing dependencies
        foreach (var module in moduleList)
        {
            var missingDeps = module.Dependencies
                .Where(dep => !moduleIds.Contains(dep))
                .ToArray();

            if (missingDeps.Length > 0)
            {
                throw new ModuleDependencyException(module.ModuleId, missingDeps);
            }
        }

        // Check for circular dependencies using depth-first search
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var recursionStack = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var module in moduleList)
        {
            if (HasCircularDependency(module.ModuleId, moduleList, visited, recursionStack, out var cycle))
            {
                throw new CircularDependencyException(cycle!);
            }
        }
    }

    private bool HasCircularDependency(
        string moduleId,
        List<IHisModule> modules,
        HashSet<string> visited,
        HashSet<string> recursionStack,
        out string[]? cycle)
    {
        cycle = null;

        if (recursionStack.Contains(moduleId))
        {
            // Circular dependency detected
            cycle = [.. recursionStack, moduleId];
            return true;
        }

        if (visited.Contains(moduleId))
        {
            return false;
        }

        visited.Add(moduleId);
        recursionStack.Add(moduleId);

        var module = modules.FirstOrDefault(m => m.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase));
        if (module != null)
        {
            foreach (var dependency in module.Dependencies)
            {
                if (HasCircularDependency(dependency, modules, visited, recursionStack, out cycle))
                {
                    return true;
                }
            }
        }

        recursionStack.Remove(moduleId);
        return false;
    }

    public IEnumerable<IHisModule> SortByDependencies(IEnumerable<IHisModule> modules)
    {
        var moduleList = modules.ToList();
        var sorted = new List<IHisModule>();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void Visit(IHisModule module)
        {
            if (visited.Contains(module.ModuleId))
            {
                return;
            }

            visited.Add(module.ModuleId);

            // Visit dependencies first
            foreach (var depId in module.Dependencies)
            {
                var dependency = moduleList.FirstOrDefault(m =>
                    m.ModuleId.Equals(depId, StringComparison.OrdinalIgnoreCase));

                if (dependency != null)
                {
                    Visit(dependency);
                }
            }

            sorted.Add(module);
        }

        foreach (var module in moduleList)
        {
            Visit(module);
        }

        return sorted;
    }

    public IEnumerable<Assembly> GetApplicationAssemblies()
    {
        return GetActiveModules()
            .Select(m => m.GetType().Assembly.GetName().Name)
            .Where(name => name?.EndsWith(".Application") == true)
            .Select(name => AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == name))
            .Where(a => a != null)
            .Cast<Assembly>();
    }

    public IEnumerable<Assembly> GetInfrastructureAssemblies()
    {
        return GetActiveModules()
            .Select(m => m.GetType().Assembly.GetName().Name)
            .Where(name => name?.EndsWith(".Infrastructure") == true)
            .Select(name => AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == name))
            .Where(a => a != null)
            .Cast<Assembly>();
    }

    public IEnumerable<Assembly> GetUIAssemblies()
    {
        return GetActiveModules()
            .SelectMany(m => m.GetUIAssemblies())
            .Distinct();
    }

    /// <summary>
    /// Loads module assemblies from the application's bin directory.
    /// This ensures assemblies are loaded even if they haven't been referenced yet.
    /// </summary>
    private void LoadModuleAssemblies()
    {
        try
        {
            // Get the directory where the application is running
            var baseDirectory = AppContext.BaseDirectory;
            
            // Find all HIS.*.Infrastructure.dll files (module entry points)
            var dllFiles = Directory.GetFiles(baseDirectory, "HIS.*.Infrastructure.dll", SearchOption.TopDirectoryOnly);
            
            _logger.LogInformation("Found {Count} module DLLs in {Directory}", dllFiles.Length, baseDirectory);
            
            foreach (var dllPath in dllFiles)
            {
                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(dllPath);
                    
                    // Check if already loaded
                    if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == assemblyName.Name))
                    {
                        _logger.LogDebug("Assembly {AssemblyName} already loaded", assemblyName.Name);
                        continue;
                    }
                    
                    // Load the assembly
                    var assembly = Assembly.Load(assemblyName);
                    _logger.LogInformation("Loaded module assembly: {AssemblyName}", assemblyName.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not load assembly from {DllPath}", dllPath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading module assemblies");
        }
    }
}
