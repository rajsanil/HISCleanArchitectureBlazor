using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Entities;

namespace HIS.MasterData.Domain.Entities;

public class Department : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int FacilityId { get; set; }
    public int? ParentDepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
    public string TenantId { get; set; } = string.Empty;

    public virtual Facility? Facility { get; set; }
    public virtual Department? ParentDepartment { get; set; }
    public virtual ICollection<Department> SubDepartments { get; set; } = new HashSet<Department>();
}
