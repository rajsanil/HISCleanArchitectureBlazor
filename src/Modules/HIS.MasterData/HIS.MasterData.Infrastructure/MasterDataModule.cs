using System.Reflection;
using FluentValidation;
using HIS.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.MasterData.Infrastructure;

/// <summary>
/// Master Data module implementation.
/// Provides foundational lookup tables used across all other HIS modules.
/// </summary>
public class MasterDataModule : IHisModule
{
    public string ModuleId => "HIS.MasterData";

    public string DisplayName => "Master Data Management";

    public string Version => "1.0.0";

    public string[] Dependencies => []; // No dependencies - this is a foundational module

    public void ConfigureDomainServices(IServiceCollection services)
    {
        // No domain services needed for this module
    }

    public void ConfigureApplicationServices(IServiceCollection services)
    {
        var assembly = typeof(HIS.MasterData.Application.Common.Interfaces.IMasterDataDbContext).Assembly;

        // Register MediatR handlers from the MasterData Application assembly
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        // Register FluentValidation validators from the MasterData Application assembly
        services.AddValidatorsFromAssembly(assembly);
    }

    public void ConfigureInfrastructureServices(IServiceCollection services, IConfiguration configuration)
    {
        // No infrastructure services needed for this module
        // All data access goes through IApplicationDbContext
    }

    public void ConfigureUIServices(IServiceCollection services)
    {
        // No special UI services needed
    }

    public void ConfigureDatabase(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDataModule).Assembly);
    }

    public IEnumerable<ModuleMenuSection> GetMenuSections()
    {
        return
        [
            // Master Data - Single unified section
            new ModuleMenuSection
            {
                Title = "Master Data",
                Roles = ["Admin", "Users", "Basic"],
                Items =
                [
                    // Master Setup - Hub page with navigation cards
                    new ModuleMenuItem
                    {
                        Title = "Master Setup",
                        Icon = "mdi-view-dashboard-outline",
                        Href = "/pages/master-setup",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    // Medical Business Setup
                    new ModuleMenuItem
                    {
                        Title = "Facilities",
                        Icon = "mdi-hospital-building",
                        Href = "/pages/facilities",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Departments",
                        Icon = "mdi-office-building-outline",
                        Href = "/pages/departments",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Shifts",
                        Icon = "mdi-clock-outline",
                        Href = "/pages/shifts",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Locations",
                        Icon = "mdi-map-marker",
                        Href = "/pages/locations",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Specialties",
                        Icon = "mdi-medical-bag",
                        Href = "/pages/specialties",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Beds",
                        Icon = "mdi-bed",
                        Href = "/pages/beds",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Bed Board",
                        Icon = "mdi-view-dashboard",
                        Href = "/pages/bedboard",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    // Geographic and Demographics
                    new ModuleMenuItem
                    {
                        Title = "Countries",
                        Icon = "mdi-earth",
                        Href = "/pages/countries",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Cities",
                        Icon = "mdi-city",
                        Href = "/pages/cities",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Nationalities",
                        Icon = "mdi-passport",
                        Href = "/pages/nationalities",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Blood Groups",
                        Icon = "mdi-water",
                        Href = "/pages/bloodgroups",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Marital Statuses",
                        Icon = "mdi-ring",
                        Href = "/pages/maritalstatuses",
                        Roles = ["Admin", "Users", "Basic"]
                    },
                    new ModuleMenuItem
                    {
                        Title = "Contacts",
                        Icon = "mdi-account-multiple",
                        Href = "/pages/contacts",
                        Roles = ["Admin", "Users", "Basic"]
                    }
                ]
            }
        ];
    }

    public IEnumerable<Type> GetPermissionTypes()
    {
        // Return permission types defined in this module
        // These will be discovered via reflection and registered as policies
        return new[]
        {
            typeof(Permissions.MasterDataPermissions)
        };
    }

    public IEnumerable<Assembly> GetUIAssemblies()
    {
        // Return the UI assembly for this module (Razor Class Library)
        // Use Assembly.Load to avoid circular dependency
        try
        {
            var uiAssembly = Assembly.Load("HIS.MasterData.UI");
            return new[] { uiAssembly };
        }
        catch
        {
            // If the UI assembly is not found, return an empty collection
            return Enumerable.Empty<Assembly>();
        }
    }
}
