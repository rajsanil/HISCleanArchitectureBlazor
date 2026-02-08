#nullable disable

namespace CleanArchitecture.Blazor.Application.Features.BloodGroups.Caching;

public static class BloodGroupCacheKey
{
    public const string GetAllCacheKey = "all-BloodGroups";
    
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"BloodGroupCacheKey:BloodGroupsWithPaginationQuery,{parameters}";
    }
    
    public static string GetByIdCacheKey(string parameters)
    {
        return $"BloodGroupCacheKey:GetByIdCacheKey,{parameters}";
    }
    
    public static IEnumerable<string>? Tags => new string[] { "bloodgroup" };
    
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}
