#nullable disable

namespace CleanArchitecture.Blazor.Application.Features.Cities.Caching;

public static class CityCacheKey
{
    public const string GetAllCacheKey = "all-Cities";
    
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"CityCacheKey:CitiesWithPaginationQuery,{parameters}";
    }
    
    public static string GetByIdCacheKey(string parameters)
    {
        return $"CityCacheKey:GetByIdCacheKey,{parameters}";
    }
    
    public static IEnumerable<string>? Tags => new string[] { "city" };
    
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}
