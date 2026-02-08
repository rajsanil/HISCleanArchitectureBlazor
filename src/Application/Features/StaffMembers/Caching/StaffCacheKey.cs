namespace CleanArchitecture.Blazor.Application.Features.StaffMembers.Caching;

public static class StaffCacheKey
{
    public const string GetAllCacheKey = "all-StaffMembers";
    public static string GetStaffByIdCacheKey(int id) => $"GetStaffById,{id}";
    public static IEnumerable<string>? Tags => new[] { "staff" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
