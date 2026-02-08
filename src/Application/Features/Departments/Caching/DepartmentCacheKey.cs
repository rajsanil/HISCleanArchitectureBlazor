namespace CleanArchitecture.Blazor.Application.Features.Departments.Caching;

public static class DepartmentCacheKey
{
    public const string GetAllCacheKey = "all-Departments";
    public static string GetDepartmentByIdCacheKey(int id) => $"GetDepartmentById,{id}";
    public static string GetPaginationCacheKey(string parameters) => $"DepartmentsWithPaginationQuery,{parameters}";
    public static IEnumerable<string>? Tags => new[] { "department" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
