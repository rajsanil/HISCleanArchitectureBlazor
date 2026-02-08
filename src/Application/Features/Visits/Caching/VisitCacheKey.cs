namespace CleanArchitecture.Blazor.Application.Features.Visits.Caching;

public static class VisitCacheKey
{
    public const string GetAllCacheKey = "all-Visits";
    public static string GetVisitByIdCacheKey(int id) => $"GetVisitById,{id}";
    public static string GetPaginationCacheKey(string parameters) => $"VisitsWithPaginationQuery,{parameters}";
    public static IEnumerable<string>? Tags => new[] { "visit" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
