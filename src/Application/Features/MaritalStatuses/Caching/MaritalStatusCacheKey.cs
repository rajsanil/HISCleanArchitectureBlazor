#nullable disable

namespace CleanArchitecture.Blazor.Application.Features.MaritalStatuses.Caching;

public static class MaritalStatusCacheKey
{
    public const string GetAllCacheKey = "all-MaritalStatuses";
    
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"MaritalStatusCacheKey:MaritalStatusesWithPaginationQuery,{parameters}";
    }
    
    public static string GetByIdCacheKey(string parameters)
    {
        return $"MaritalStatusCacheKey:GetByIdCacheKey,{parameters}";
    }
    
    public static IEnumerable<string>? Tags => new string[] { "maritalstatus" };
    
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}
