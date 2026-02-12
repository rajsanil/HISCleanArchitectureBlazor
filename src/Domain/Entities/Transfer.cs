using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Transfer : BaseAuditableEntity, IMustHaveTenant
{
    public int VisitId { get; set; }
    public int? FromBedId { get; set; }
    public int? ToBedId { get; set; }
    public int? FromLocationId { get; set; }
    public int? ToLocationId { get; set; }
    public DateTime TransferDate { get; set; }
    public string? Reason { get; set; }
    public int? OrderedByDoctorId { get; set; }
    public string TenantId { get; set; } = string.Empty;

    public virtual Visit? Visit { get; set; }
    public virtual Staff? OrderedByDoctor { get; set; }
}
