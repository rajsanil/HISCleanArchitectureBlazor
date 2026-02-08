using CleanArchitecture.Blazor.Infrastructure.Constants.Role;
using CleanArchitecture.Blazor.Server.UI.Models.NavigationMenu;

namespace CleanArchitecture.Blazor.Server.UI.Services.Navigation;

public class MenuService : IMenuService
{
    private readonly List<MenuSectionModel> _features = new()
    {
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
        new MenuSectionModel
        {
            Title = "Hospital",
            Roles = new[] { RoleName.Admin, RoleName.Users },
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    IsParent = true,
                    Title = "Foundation",
                    Icon = Icons.Material.Filled.Business,
                    PageStatus = PageStatus.Completed,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Facilities",
                            Href = "/pages/facilities",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Departments",
                            Href = "/pages/departments",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Specialties",
                            Href = "/pages/specialties",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Locations",
                            Href = "/pages/locations",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Rooms",
                            Href = "/pages/rooms",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Beds",
                            Href = "/pages/beds",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Staff",
                            Href = "/pages/staff",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    IsParent = true,
                    Title = "Patient Management",
                    Icon = Icons.Material.Filled.People,
                    PageStatus = PageStatus.Completed,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Patients",
                            Href = "/pages/patients",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Visits",
                            Href = "/pages/visits",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    IsParent = true,
                    Title = "Clinical",
                    Icon = Icons.Material.Filled.MedicalServices,
                    PageStatus = PageStatus.Completed,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Encounters",
                            Href = "/pages/encounters",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Bed Board",
                            Href = "/pages/bedboard",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    IsParent = true,
                    Title = "Master Data",
                    Icon = Icons.Material.Filled.Dataset,
                    PageStatus = PageStatus.Completed,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Countries",
                            Href = "/pages/countries",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Cities",
                            Href = "/pages/cities",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Nationalities",
                            Href = "/pages/nationalities",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Blood Groups",
                            Href = "/pages/bloodgroups",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Marital Statuses",
                            Href = "/pages/maritalstatuses",
                            PageStatus = PageStatus.Completed
                        }
                    }
                }
            }
        },
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

    public IEnumerable<MenuSectionModel> Features => _features;
}