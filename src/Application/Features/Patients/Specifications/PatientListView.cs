namespace CleanArchitecture.Blazor.Application.Features.Patients.Specifications;

public enum PatientListView
{
    [Description("All")] All,
    [Description("My Patients")] My,
    [Description("Registered Today")] TODAY,
    [Description("Last 30 Days")] LAST_30_DAYS,
    [Description("VIP Only")] VIPOnly
}
