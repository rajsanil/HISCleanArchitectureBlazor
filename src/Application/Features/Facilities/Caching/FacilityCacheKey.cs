namespace CleanArchitecture.Blazor.Application.Features.Facilities.Caching;

public static class FacilityCacheKey
{
    public const string GetAllCacheKey = "all-Facilities";
    public static string GetFacilityByIdCacheKey(int id) => $"GetFacilityById,{id}";
    public static string GetPaginationCacheKey(string parameters) => $"FacilitiesWithPaginationQuery,{parameters}";
    public static IEnumerable<string>? Tags => new[] { "facility" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
