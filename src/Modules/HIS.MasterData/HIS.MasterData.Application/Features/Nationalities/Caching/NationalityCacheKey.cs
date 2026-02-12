namespace HIS.MasterData.Application.Features.Nationalities.Caching;

public static class NationalityCacheKey
{
    public const string GetAllCacheKey = "all-Nationalities";
    public static string GetNationalityByIdCacheKey(int id) => $"GetNationalityById,{id}";
    public static IEnumerable<string>? Tags => new[] { "nationality" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
