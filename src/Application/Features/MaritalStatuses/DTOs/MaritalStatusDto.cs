namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.DTOs;

[Description("Marital Statuses")]
public class MaritalStatusDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("Display Order")] public int DisplayOrder { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
