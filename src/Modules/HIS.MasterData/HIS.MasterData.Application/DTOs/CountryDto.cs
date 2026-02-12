namespace HIS.MasterData.Application.DTOs;

public class CountryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public string? Iso2Code { get; set; }
    public string? Iso3Code { get; set; }
    public string? PhoneCode { get; set; }
    public bool IsActive { get; set; } = true;
}
