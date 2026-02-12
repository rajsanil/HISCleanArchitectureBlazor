namespace HIS.MasterData.Application.Features.Locations.DTOs;

[Description("Locations")]
public class LocationDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Location Type")] public string LocationType { get; set; } = string.Empty;
    [Description("Department Id")] public int DepartmentId { get; set; }
    [Description("Facility Id")] public int FacilityId { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
