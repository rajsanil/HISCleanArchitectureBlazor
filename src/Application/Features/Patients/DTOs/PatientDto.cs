namespace CleanArchitecture.Blazor.Application.Features.Patients.DTOs;

[Description("Patients")]
public class PatientDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("MRN")] public string MRN { get; set; } = string.Empty;
    [Description("First Name")] public string FirstName { get; set; } = string.Empty;
    [Description("Middle Name")] public string? MiddleName { get; set; }
    [Description("Last Name")] public string LastName { get; set; } = string.Empty;
    [Description("First Name (Arabic)")] public string? FirstNameArabic { get; set; }
    [Description("Last Name (Arabic)")] public string? LastNameArabic { get; set; }
    [Description("Date of Birth")] public DateTime DateOfBirth { get; set; }
    [Description("Gender")] public string Gender { get; set; } = string.Empty;
    [Description("Nationality")] public int? NationalityId { get; set; }
    [Description("Emirates ID")] public string? EmiratesId { get; set; }
    [Description("Passport Number")] public string? PassportNumber { get; set; }
    [Description("Blood Group")] public int? BloodGroupId { get; set; }
    [Description("Marital Status")] public int? MaritalStatusId { get; set; }
    [Description("Phone")] public string? Phone { get; set; }
    [Description("Email")] public string? Email { get; set; }
    [Description("Address")] public string? Address { get; set; }
    [Description("City")] public int? CityId { get; set; }
    [Description("Country")] public int? CountryId { get; set; }
    [Description("Photo URL")] public string? PhotoUrl { get; set; }
    [Description("VIP")] public bool IsVIP { get; set; }
    [Description("Deceased")] public bool IsDeceased { get; set; }
    [Description("Deceased Date")] public DateTime? DeceasedDate { get; set; }
    [Description("Active")] public bool IsActive { get; set; }
}
