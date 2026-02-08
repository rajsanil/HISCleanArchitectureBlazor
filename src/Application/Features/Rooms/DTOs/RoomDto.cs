namespace CleanArchitecture.Blazor.Application.Features.Rooms.DTOs;

[Description("Rooms")]
public class RoomDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("Code")] public string Code { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("Room Type")] public string RoomType { get; set; } = string.Empty;
    [Description("Location Id")] public int LocationId { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
