using HIS.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Core.Infrastructure;

/// <summary>
/// Extension methods for registering module infrastructure services.
/// </summary>
public static class ModuleServiceCollectionExtensions
{
    /// <summary>
    /// Adds module loading and discovery services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddModuleInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IModuleLoader, ModuleLoader>();
        return services;
    }
}
