namespace CleanArchitecture.Blazor.Application.Features.Patients.Specifications;

public class PatientAdvancedFilter : PaginationFilter
{
    public string? Gender { get; set; }
    public bool? IsVIP { get; set; }
    public bool? IsActive { get; set; }
    public string? MRN { get; set; }
    public string? EmiratesId { get; set; }
    public string? Phone { get; set; }
    public PatientListView ListView { get; set; } = PatientListView.All;
    public UserProfile? CurrentUser { get; set; }
}
