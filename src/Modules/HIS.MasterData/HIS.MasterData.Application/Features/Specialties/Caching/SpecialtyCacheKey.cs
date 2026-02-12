namespace HIS.MasterData.Application.Features.Specialties.Caching;

public static class SpecialtyCacheKey
{
    public const string GetAllCacheKey = "all-Specialties";
    public static string GetSpecialtyByIdCacheKey(int id) => $"GetSpecialtyById,{id}";
    public static IEnumerable<string>? Tags => new[] { "specialty" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
