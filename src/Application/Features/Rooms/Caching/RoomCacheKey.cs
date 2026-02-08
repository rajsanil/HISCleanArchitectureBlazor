namespace CleanArchitecture.Blazor.Application.Features.Rooms.Caching;

public static class RoomCacheKey
{
    public const string GetAllCacheKey = "all-Rooms";
    public static string GetRoomByIdCacheKey(int id) => $"GetRoomById,{id}";
    public static IEnumerable<string>? Tags => new[] { "room" };
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);
}
