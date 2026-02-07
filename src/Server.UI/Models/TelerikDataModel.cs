namespace CleanArchitecture.Blazor.Server.UI.Models;

/// <summary>
/// Sample data model for Telerik Grid demonstration
/// </summary>
public class TelerikDataModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; } = string.Empty;
}
