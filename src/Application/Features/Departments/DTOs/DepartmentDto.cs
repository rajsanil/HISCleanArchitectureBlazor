namespace CleanArchitecture.Blazor.Application.Features.Departments.DTOs;

[Description("Departments")]
public class DepartmentDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Name (Arabic)")] public string? NameArabic { get; set; }
    [Description("Facility Id")] public int FacilityId { get; set; }
    [Description("Parent Department")] public int? ParentDepartmentId { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
