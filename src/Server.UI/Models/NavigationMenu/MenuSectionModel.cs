namespace CleanArchitecture.Blazor.Server.UI.Models.NavigationMenu;

public class MenuSectionModel
{
    public string Title { get; set; } = string.Empty;
    public string[]? Roles { get; set; }
    /// <summary>
    /// Permission required to see this section. Takes precedence over Roles when set.
    /// </summary>
    public string? Permission { get; set; }
    public IList<MenuSectionItemModel>? SectionItems { get; set; }
}