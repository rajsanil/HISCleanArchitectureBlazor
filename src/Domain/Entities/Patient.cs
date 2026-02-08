using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class Patient : BaseAuditableSoftDeleteEntity, IMustHaveTenant
{
    public string MRN { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? FirstNameArabic { get; set; }
    public string? LastNameArabic { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int? NationalityId { get; set; }
    public string? EmiratesId { get; set; }
    public string? PassportNumber { get; set; }
    public int? BloodGroupId { get; set; }
    public int? MaritalStatusId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public int? CountryId { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsVIP { get; set; }
    public bool IsDeceased { get; set; }
    public DateTime? DeceasedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string TenantId { get; set; } = string.Empty;

    public virtual Nationality? Nationality { get; set; }
    public virtual BloodGroup? BloodGroup { get; set; }
    public virtual MaritalStatus? MaritalStatus { get; set; }
    public virtual City? City { get; set; }
    public virtual Country? Country { get; set; }
    public virtual ICollection<PatientContact> Contacts { get; set; } = new HashSet<PatientContact>();
    public virtual ICollection<Visit> Visits { get; set; } = new HashSet<Visit>();

    public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
}
