namespace CleanArchitecture.Blazor.Application.Features.Visits.DTOs;

[Description("Visits")]
public class VisitDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Visit Number")] public string VisitNumber { get; set; } = string.Empty;
    [Description("Patient Id")] public int PatientId { get; set; }
    [Description("Patient Name")] public string? PatientName { get; set; }
    [Description("Visit Type")] public string VisitType { get; set; } = string.Empty;
    [Description("Visit Status")] public string VisitStatus { get; set; } = string.Empty;
    [Description("Facility Id")] public int FacilityId { get; set; }
    [Description("Department Id")] public int? DepartmentId { get; set; }
    [Description("Attending Doctor Id")] public int? AttendingDoctorId { get; set; }
    [Description("Registration Date")] public DateTime RegistrationDate { get; set; }
    [Description("Discharge Date")] public DateTime? DischargeDate { get; set; }
    [Description("Discharge Disposition")] public string? DischargeDisposition { get; set; }
}
