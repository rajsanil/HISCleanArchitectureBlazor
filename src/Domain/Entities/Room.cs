using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Room : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty; // Single, Double, ICU, OT
    public int LocationId { get; set; }
    public bool IsActive { get; set; } = true;
    public string TenantId { get; set; } = string.Empty;
}
