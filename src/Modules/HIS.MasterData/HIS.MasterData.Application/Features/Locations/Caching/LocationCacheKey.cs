namespace HIS.MasterData.Application.Features.Locations.Caching;

public static class LocationCacheKey
{
    public const string GetAllCacheKey = "all-Locations";
    public static string GetLocationByIdCacheKey(int id) => $"GetLocationById,{id}";
    public static IEnumerable<string>? Tags => new[] { "location" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
