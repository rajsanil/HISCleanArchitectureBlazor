namespace HIS.MasterData.Application.DTOs;

public class MaritalStatusDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
