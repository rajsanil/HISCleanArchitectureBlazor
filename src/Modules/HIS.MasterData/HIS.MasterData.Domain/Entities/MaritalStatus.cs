using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace HIS.MasterData.Domain.Entities;

public class MaritalStatus : BaseAuditableSoftDeleteEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? NameArabic { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
