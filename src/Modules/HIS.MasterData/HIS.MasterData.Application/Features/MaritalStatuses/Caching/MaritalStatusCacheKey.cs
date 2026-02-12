namespace HIS.MasterData.Application.Features.MaritalStatuses.Caching;

public static class MaritalStatusCacheKey
{
    public const string GetAllCacheKey = "all-MaritalStatuses";
    public static string GetMaritalStatusByIdCacheKey(int id) => $"GetMaritalStatusById,{id}";
    public static IEnumerable<string>? Tags => new[] { "maritalstatus" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
