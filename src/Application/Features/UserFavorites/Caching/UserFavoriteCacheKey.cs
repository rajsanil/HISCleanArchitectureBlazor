// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.UserFavorites.Caching;

public static class UserFavoriteCacheKey
{
    private const string CacheKeyPrefix = "UserFavorites";
    public static string GetCacheKey(string userId) => $"{CacheKeyPrefix}:{userId}";
    public static string GetAllCacheKey => CacheKeyPrefix;
    public static IEnumerable<string> Tags => new[] { CacheKeyPrefix };
}
