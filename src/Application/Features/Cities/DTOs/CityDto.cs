namespace CleanArchitecture.Blazor.Application.Features.Cities.DTOs;

[Description("Cities")]
public class CityDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("Country")] public int? CountryId { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
