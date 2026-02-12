using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Admission : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public int VisitId { get; set; }
    public DateTime AdmissionDate { get; set; }
    public int? BedId { get; set; }
    public int? RoomId { get; set; }
    public int? LocationId { get; set; }
    public int? AdmittingDoctorId { get; set; }
    public string AdmissionType { get; set; } = string.Empty; // Elective, Emergency
    public DateTime? ExpectedDischargeDate { get; set; }
    public string TenantId { get; set; } = string.Empty;

    public virtual Visit? Visit { get; set; }
    public virtual Room? Room { get; set; }
    public virtual Staff? AdmittingDoctor { get; set; }
}
