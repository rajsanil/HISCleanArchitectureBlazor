namespace HIS.MasterData.Application.Features.Beds.Caching;

public static class BedCacheKey
{
    public const string GetAllCacheKey = "all-Beds";
    public static string GetBedByIdCacheKey(int id) => $"GetBedById,{id}";
    public static IEnumerable<string>? Tags => new[] { "bed" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
