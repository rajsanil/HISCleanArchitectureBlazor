namespace HIS.MasterData.Application.Features.Cities.Caching;

public static class CityCacheKey
{
    public const string GetAllCacheKey = "all-Cities";
    public static string GetCityByIdCacheKey(int id) => $"GetCityById,{id}";
    public static IEnumerable<string>? Tags => new[] { "city" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
