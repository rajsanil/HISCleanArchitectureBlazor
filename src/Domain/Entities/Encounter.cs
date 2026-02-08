using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Encounter : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string EncounterNumber { get; set; } = string.Empty;
    public int VisitId { get; set; }
    public string EncounterType { get; set; } = string.Empty; // Consultation, Procedure, FollowUp
    public string EncounterStatus { get; set; } = "Planned"; // Planned, InProgress, Completed, Cancelled
    public int DoctorId { get; set; }
    public int? DepartmentId { get; set; }
    public int? LocationId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Notes { get; set; }
    public string TenantId { get; set; } = string.Empty;

    public virtual Visit? Visit { get; set; }
    public virtual Staff? Doctor { get; set; }
    public virtual Department? Department { get; set; }
    public virtual Location? Location { get; set; }
}
