using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Discharge : BaseAuditableEntity, IMustHaveTenant
{
    public int VisitId { get; set; }
    public DateTime DischargeDate { get; set; }
    public string DischargeDisposition { get; set; } = string.Empty;
    public string? DischargeSummary { get; set; }
    public int? DischargedByDoctorId { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public string? FollowUpNotes { get; set; }
    public string TenantId { get; set; } = string.Empty;

    public virtual Visit? Visit { get; set; }
    public virtual Staff? DischargedByDoctor { get; set; }
}
