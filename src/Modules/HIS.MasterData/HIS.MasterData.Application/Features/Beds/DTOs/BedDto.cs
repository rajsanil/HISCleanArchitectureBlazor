namespace HIS.MasterData.Application.Features.Beds.DTOs;

[Description("Beds")]
public class BedDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Room Id")] public int RoomId { get; set; }
    [Description("Bed Status")] public string BedStatus { get; set; } = "Available";
    [Description("Active")] public bool IsActive { get; set; }
}
