namespace CleanArchitecture.Blazor.Application.Features.Patients.Specifications;

#nullable disable warnings
public class PatientAdvancedSpecification : Specification<Patient>
{
    public PatientAdvancedSpecification(PatientAdvancedFilter filter)
    {
        DateTime today = DateTime.UtcNow;
        var todayRange = today.GetDateRange("TODAY", filter.CurrentUser?.LocalTimeOffset ?? TimeSpan.Zero);
        var last30DaysRange = today.GetDateRange("LAST_30_DAYS", filter.CurrentUser?.LocalTimeOffset ?? TimeSpan.Zero);

        Query.Where(x => x.FirstName != null)
            .Where(x => x.FirstName.Contains(filter.Keyword) ||
                        x.LastName.Contains(filter.Keyword) ||
                        x.MRN.Contains(filter.Keyword) ||
                        x.EmiratesId.Contains(filter.Keyword) ||
                        x.Phone.Contains(filter.Keyword),
                   !string.IsNullOrEmpty(filter.Keyword))
            .Where(x => x.MRN.Contains(filter.MRN), !string.IsNullOrEmpty(filter.MRN))
            .Where(x => x.EmiratesId.Contains(filter.EmiratesId), !string.IsNullOrEmpty(filter.EmiratesId))
            .Where(x => x.Phone.Contains(filter.Phone), !string.IsNullOrEmpty(filter.Phone))
            .Where(x => x.Gender == filter.Gender, !string.IsNullOrEmpty(filter.Gender))
            .Where(x => x.IsVIP == filter.IsVIP.Value, filter.IsVIP.HasValue)
            .Where(x => x.IsActive == filter.IsActive.Value, filter.IsActive.HasValue)
            .Where(x => x.CreatedBy == filter.CurrentUser.UserId, filter.ListView == PatientListView.My)
            .Where(x => x.IsVIP, filter.ListView == PatientListView.VIPOnly)
            .Where(x => x.Created >= todayRange.Start && x.Created < todayRange.End.AddDays(1), filter.ListView == PatientListView.TODAY)
            .Where(x => x.Created >= last30DaysRange.Start, filter.ListView == PatientListView.LAST_30_DAYS);
    }
}
