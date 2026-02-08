using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Location : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LocationType { get; set; } = string.Empty; // Ward, Clinic, ER, OT, ICU
    public int? DepartmentId { get; set; }
    public int FacilityId { get; set; }
    public bool IsActive { get; set; } = true;
    public string TenantId { get; set; } = string.Empty;

    public virtual Facility? Facility { get; set; }
    public virtual Department? Department { get; set; }
    public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
}
