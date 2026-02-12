namespace HIS.MasterData.Application.DTOs;

public class CityDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int CountryId { get; set; }
    public string? CountryName { get; set; }
    public bool IsActive { get; set; } = true;
}
