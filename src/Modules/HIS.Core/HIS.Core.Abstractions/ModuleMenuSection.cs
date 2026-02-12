namespace HIS.Core.Abstractions;

/// <summary>
/// Represents a menu section contributed by a module.
/// </summary>
public class ModuleMenuSection
{
    public string Title { get; set; } = string.Empty;
    public string[]? Roles { get; set; }
    public List<ModuleMenuItem> Items { get; set; } = [];
}

/// <summary>
/// Represents a menu item within a module's menu section.
/// </summary>
public class ModuleMenuItem
{
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string? Href { get; set; }
    public string[]? Roles { get; set; }
    public PageStatus? PageStatus { get; set; }
    public List<ModuleMenuSubItem>? SubItems { get; set; }
}

/// <summary>
/// Represents a sub-menu item (child of a menu item).
/// </summary>
public class ModuleMenuSubItem
{
    public string Title { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public string[]? Roles { get; set; }
    public PageStatus? PageStatus { get; set; }
}

/// <summary>
/// Status indicator for pages under development.
/// </summary>
public enum PageStatus
{
    Completed,
    Wip,
    New,
    ComingSoon
}
