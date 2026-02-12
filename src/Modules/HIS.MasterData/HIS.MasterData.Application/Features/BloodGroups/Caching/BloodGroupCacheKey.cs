namespace HIS.MasterData.Application.Features.BloodGroups.Caching;

public static class BloodGroupCacheKey
{
    public const string GetAllCacheKey = "all-BloodGroups";
    public static string GetBloodGroupByIdCacheKey(int id) => $"GetBloodGroupById,{id}";
    public static IEnumerable<string>? Tags => new[] { "bloodgroup" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
