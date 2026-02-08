using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Staff : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string? ApplicationUserId { get; set; }
    public string StaffType { get; set; } = string.Empty; // Doctor, Nurse, Technician, Admin
    public int? DepartmentId { get; set; }
    public int? SpecialtyId { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string TenantId { get; set; } = string.Empty;

    public virtual ApplicationUser? ApplicationUser { get; set; }
    public virtual Department? Department { get; set; }
    public virtual Specialty? Specialty { get; set; }

    public string FullName => $"{Title} {FirstName} {LastName}".Trim();
}
