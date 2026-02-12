using CleanArchitecture.Blazor.Infrastructure.Constants.Role;
using CleanArchitecture.Blazor.Server.UI.Models.NavigationMenu;
using HIS.Core.Abstractions;
using PageStatus = CleanArchitecture.Blazor.Server.UI.Models.NavigationMenu.PageStatus;

namespace CleanArchitecture.Blazor.Server.UI.Services.Navigation;

public class MenuService : IMenuService
{
    private readonly IModuleLoader _moduleLoader;
    private readonly List<MenuSectionModel> _features;

    public MenuService(IModuleLoader moduleLoader)
    {
        _moduleLoader = moduleLoader;
        _features = BuildFeatures();
    }

    private List<MenuSectionModel> BuildFeatures()
    {
        return new List<MenuSectionModel>
        {
            // Application Section (hardcoded)
            new MenuSectionModel
            {
            Title = "Application",
            SectionItems = new List<MenuSectionItemModel>
            {
                new() { Title = "Home", Icon = Icons.Material.Filled.Home, Href = "/" },
                new()
                {
                    Title = "E-Commerce",
                    Icon = Icons.Material.Filled.ShoppingCart,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Products",
                            Href = "/pages/products",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Documents",
                            Href = "/pages/documents",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Contacts",
                            Href = "/pages/contacts",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Telerik Data",
                            Href = "/pages/telerikdata",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    Title = "Chatbot",
                    Icon = Icons.Material.Filled.ChatBubble,
                    Href ="/ai/chatbot",
                    PageStatus = PageStatus.Completed
                },
                new()
                {
                    Title = "Analytics",
                    Roles = new[] { RoleName.Admin, RoleName.Users },
                    Icon = Icons.Material.Filled.Analytics,
                    Href = "/analytics",
                    PageStatus = PageStatus.ComingSoon
                },
                new()
                {
                    Title = "Banking",
                    Roles = new[] { RoleName.Admin, RoleName.Users },
                    Icon = Icons.Material.Filled.Money,
                    Href = "/banking",
                    PageStatus = PageStatus.ComingSoon
                },
                new()
                {
                    Title = "Booking",
                    Roles = new[] { RoleName.Admin, RoleName.Users },
                    Icon = Icons.Material.Filled.CalendarToday,
                    Href = "/booking",
                    PageStatus = PageStatus.ComingSoon
                }
            }
            },
            // Hospital Section (dynamically built from modules)
            BuildHospitalSection(),
            // MANAGEMENT Section (hardcoded)
            new MenuSectionModel
        {
            Title = "MANAGEMENT",
            Roles = new[] { RoleName.Admin },
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    IsParent = true,
                    Title = "Authorization",
                    Icon = Icons.Material.Filled.ManageAccounts,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Multi-Tenant",
                            Href = "/system/tenants",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Users",
                            Href = "/identity/users",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Roles",
                            Href = "/identity/roles",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Profile",
                            Href = "/user/profile",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    IsParent = true,
                    Title = "System",
                    Icon = Icons.Material.Filled.Devices,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "License",
                            Href = "/system/license",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Picklist",
                            Href = "/system/picklistset",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Audit Trails",
                            Href = "/system/audittrails",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Logs",
                            Href = "/system/logs",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Jobs",
                            Href = "/jobs",
                            PageStatus = PageStatus.Completed,
                            Target = "_blank"
                        }
                    }
                }
            }
        }
        };
    }

    /// <summary>
    /// Builds the Hospital menu section dynamically from active licensed modules.
    /// </summary>
    private MenuSectionModel BuildHospitalSection()
    {
        var hospitalSection = new MenuSectionModel
        {
            Title = "Hospital",
            Roles = new[] { RoleName.Admin, RoleName.Users },
            SectionItems = new List<MenuSectionItemModel>()
        };

        // Get all active modules and their menu contributions
        var activeModules = _moduleLoader.GetActiveModules();
        
        foreach (var module in activeModules)
        {
            var menuSections = module.GetMenuSections();
            
            foreach (var moduleSection in menuSections)
            {
                // Each ModuleMenuSection becomes a parent MenuSectionItemModel
                var sectionItem = new MenuSectionItemModel
                {
                    Title = moduleSection.Title,
                    Icon = GetIconForSection(moduleSection.Title),
                    IsParent = true,
                    Roles = moduleSection.Roles,
                    PageStatus = PageStatus.Completed,
                    MenuItems = new List<MenuSectionSubItemModel>()
                };

                // Convert module menu items to sub-items
                foreach (var item in moduleSection.Items)
                {
                    // Simple menu item - add to section
                    sectionItem.MenuItems.Add(new MenuSectionSubItemModel
                    {
                        Title = item.Title,
                        Href = item.Href,
                        Roles = item.Roles,
                        PageStatus = item.PageStatus.HasValue 
                            ? (PageStatus)(int)item.PageStatus.Value 
                            : PageStatus.Completed
                    });
                }

                hospitalSection.SectionItems.Add(sectionItem);
            }
        }

        return hospitalSection;
    }

    /// <summary>
    /// Maps module section titles to Material Design icons.
    /// </summary>
    private static string GetIconForSection(string sectionTitle)
    {
        return sectionTitle switch
        {
            "Medical Business Setup" => Icons.Material.Filled.BusinessCenter,
            "Master Data" => Icons.Material.Filled.Dataset,
            "Foundation" => Icons.Material.Filled.Business,
            "Patient Management" => Icons.Material.Filled.People,
            "Outpatient" => Icons.Material.Filled.LocalHospital,
            "Inpatient" => Icons.Material.Filled.Hotel,
            "Emergency" => Icons.Material.Filled.Emergency,
            "Clinical" => Icons.Material.Filled.MedicalServices,
            "Laboratory" => Icons.Material.Filled.Science,
            "Radiology" => Icons.Material.Filled.Biotech,
            "Pharmacy" => Icons.Material.Filled.LocalPharmacy,
            "Nursing" => Icons.Material.Filled.HealthAndSafety,
            "Inventory" => Icons.Material.Filled.Inventory,
            "Insurance" => Icons.Material.Filled.AssuredWorkload,
            "MRD" => Icons.Material.Filled.Folder,
            "Support" => Icons.Material.Filled.Support,
            _ => Icons.Material.Filled.Category
        };
    }

    public IEnumerable<MenuSectionModel> Features => _features;
}