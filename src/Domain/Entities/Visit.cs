using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Visit : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string VisitNumber { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public string VisitType { get; set; } = string.Empty; // OPD, IPD, ER, DayCare
    public string VisitStatus { get; set; } = "Registered"; // Registered, InProgress, Discharged, Cancelled
    public int FacilityId { get; set; }
    public int? DepartmentId { get; set; }
    public int? AttendingDoctorId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string? DischargeDisposition { get; set; }
    public string TenantId { get; set; } = string.Empty;

    public virtual Patient? Patient { get; set; }
    public virtual Facility? Facility { get; set; }
    public virtual Staff? AttendingDoctor { get; set; }
    public virtual Admission? Admission { get; set; }
    public virtual Discharge? Discharge { get; set; }
    public virtual ICollection<Encounter> Encounters { get; set; } = new HashSet<Encounter>();
    public virtual ICollection<Transfer> Transfers { get; set; } = new HashSet<Transfer>();
}
