namespace CleanArchitecture.Blazor.Application.Features.Encounters.Caching;

public static class EncounterCacheKey
{
    public const string GetAllCacheKey = "all-Encounters";
    public static string GetEncounterByIdCacheKey(int id) => $"GetEncounterById,{id}";
    public static IEnumerable<string>? Tags => new[] { "encounter" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
