namespace HIS.MasterData.Application.Features.Rooms.DTOs;

/// <summary>
/// Temporary stub DTO for Room until cross-module communication is implemented.
/// TODO: Replace with cross-module service or move Room entity to MasterData module.
/// </summary>
public class RoomDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public bool IsActive { get; set; }
}
