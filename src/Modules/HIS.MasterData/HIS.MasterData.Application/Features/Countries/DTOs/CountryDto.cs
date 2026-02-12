namespace HIS.MasterData.Application.Features.Countries.DTOs;

[Description("Countries")]
public class CountryDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("ISO2 Code")] public string? Iso2Code { get; set; }
    [Description("ISO3 Code")] public string? Iso3Code { get; set; }
    [Description("Phone Code")] public string? PhoneCode { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
