using HIS.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Core.Licensing;

/// <summary>
/// Extension methods for registering licensing services.
/// </summary>
public static class LicensingServiceCollectionExtensions
{
    /// <summary>
    /// Adds licensing services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddLicensing(
        this IServiceCollection services,
        Action<LicenseOptions>? configure = null)
    {
        if (configure != null)
        {
            services.Configure(configure);
        }
        else
        {
            services.Configure<LicenseOptions>(options =>
            {
                options.LicenseFilePath = "license.json";
                options.AllowUnlicensed = false;
            });
        }

        services.AddSingleton<LicenseValidator>();
        services.AddSingleton<ILicenseService, LicenseService>();

        return services;
    }
}
