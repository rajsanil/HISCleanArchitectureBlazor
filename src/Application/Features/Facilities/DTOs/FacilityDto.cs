namespace CleanArchitecture.Blazor.Application.Features.Facilities.DTOs;

[Description("Facilities")]
public class FacilityDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("License Number")] public string? LicenseNumber { get; set; }
    [Description("Address")] public string? Address { get; set; }
    [Description("Phone")] public string? Phone { get; set; }
    [Description("Email")] public string? Email { get; set; }
}
