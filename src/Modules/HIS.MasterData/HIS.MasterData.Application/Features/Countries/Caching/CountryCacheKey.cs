namespace HIS.MasterData.Application.Features.Countries.Caching;

public static class CountryCacheKey
{
    public const string GetAllCacheKey = "all-Countries";
    public static string GetCountryByIdCacheKey(int id) => $"GetCountryById,{id}";
    public static IEnumerable<string>? Tags => new[] { "country" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
