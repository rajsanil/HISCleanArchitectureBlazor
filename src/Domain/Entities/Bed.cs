using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Bed : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string Code { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public string BedStatus { get; set; } = "Available"; // Available, Occupied, Maintenance, Blocked
    public bool IsActive { get; set; } = true;
    public string TenantId { get; set; } = string.Empty;

    public virtual Room? Room { get; set; }
}
