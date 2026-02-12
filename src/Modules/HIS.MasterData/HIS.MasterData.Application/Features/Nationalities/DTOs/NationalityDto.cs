namespace HIS.MasterData.Application.Features.Nationalities.DTOs;

[Description("Nationalities")]
public class NationalityDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
