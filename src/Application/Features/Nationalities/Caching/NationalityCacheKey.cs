#nullable disable

namespace CleanArchitecture.Blazor.Application.Features.Nationalities.Caching;

public static class NationalityCacheKey
{
    public const string GetAllCacheKey = "all-Nationalities";
    
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"NationalityCacheKey:NationalitiesWithPaginationQuery,{parameters}";
    }
    
    public static string GetByIdCacheKey(string parameters)
    {
        return $"NationalityCacheKey:GetByIdCacheKey,{parameters}";
    }
    
    public static IEnumerable<string>? Tags => new string[] { "nationality" };
    
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}
