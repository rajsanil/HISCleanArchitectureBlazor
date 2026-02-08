namespace CleanArchitecture.Blazor.Application.Features.Encounters.DTOs;

[Description("Encounters")]
public class EncounterDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Encounter Number")] public string EncounterNumber { get; set; } = string.Empty;
    [Description("Visit Id")] public int VisitId { get; set; }
    [Description("Encounter Type")] public string EncounterType { get; set; } = string.Empty;
    [Description("Encounter Status")] public string EncounterStatus { get; set; } = string.Empty;
    [Description("Doctor Id")] public int? DoctorId { get; set; }
    [Description("Department Id")] public int? DepartmentId { get; set; }
    [Description("Location Id")] public int? LocationId { get; set; }
    [Description("Start Date")] public DateTime StartDate { get; set; }
    [Description("End Date")] public DateTime? EndDate { get; set; }
    [Description("Chief Complaint")] public string? ChiefComplaint { get; set; }
    [Description("Notes")] public string? Notes { get; set; }
}
