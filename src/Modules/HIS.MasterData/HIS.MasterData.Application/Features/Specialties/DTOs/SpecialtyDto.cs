namespace HIS.MasterData.Application.Features.Specialties.DTOs;

[Description("Specialties")]
public class SpecialtyDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("Department Id")] public int DepartmentId { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
