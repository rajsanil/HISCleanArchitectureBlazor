using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Country : BaseAuditableSoftDeleteEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public string? Iso2Code { get; set; }
    public string? Iso3Code { get; set; }
    public string? PhoneCode { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
}
