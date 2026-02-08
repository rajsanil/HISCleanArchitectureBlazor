namespace CleanArchitecture.Blazor.Application.Features.Patients.Caching;

public static class PatientCacheKey
{
    public const string GetAllCacheKey = "all-Patients";

    public static string GetPatientByIdCacheKey(int id)
    {
        return $"GetPatientById,{id}";
    }

    public static string GetPaginationCacheKey(string parameters)
    {
        return $"PatientsWithPaginationQuery,{parameters}";
    }

    public static IEnumerable<string>? Tags => new[] { "patient" };

    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}
