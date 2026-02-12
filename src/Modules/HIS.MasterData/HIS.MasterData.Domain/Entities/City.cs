using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace HIS.MasterData.Domain.Entities;

public class City : BaseAuditableSoftDeleteEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int? CountryId { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Country? Country { get; set; }
}
