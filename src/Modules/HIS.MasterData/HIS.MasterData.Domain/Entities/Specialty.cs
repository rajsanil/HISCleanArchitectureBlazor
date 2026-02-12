using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace HIS.MasterData.Domain.Entities;

public class Specialty : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int? DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
    public string TenantId { get; set; } = string.Empty;

    public virtual Department? Department { get; set; }
}
