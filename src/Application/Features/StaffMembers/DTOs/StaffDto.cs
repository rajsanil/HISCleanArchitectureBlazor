namespace CleanArchitecture.Blazor.Application.Features.StaffMembers.DTOs;

[Description("Staff Members")]
public class StaffDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Employee Code")] public string EmployeeCode { get; set; } = string.Empty;
    [Description("User Id")] public string? ApplicationUserId { get; set; }
    [Description("Staff Type")] public string StaffType { get; set; } = string.Empty;
    [Description("Department Id")] public int? DepartmentId { get; set; }
    [Description("Specialty Id")] public int? SpecialtyId { get; set; }
    [Description("License Number")] public string? LicenseNumber { get; set; }
    [Description("Title")] public string? Title { get; set; }
    [Description("First Name")] public string FirstName { get; set; } = string.Empty;
    [Description("Last Name")] public string LastName { get; set; } = string.Empty;
    [Description("Full Name")] public string FullName { get; set; } = string.Empty;
    [Description("Active")] public bool IsActive { get; set; }
}
