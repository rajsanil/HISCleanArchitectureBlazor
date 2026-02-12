// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Caching;

/// <summary>
/// Cache key definitions for Shift-related operations.
/// </summary>
public static class ShiftCacheKey
{
    /// <summary>
    /// Refreshes the cache by removing all shift-related cache entries.
    /// </summary>
    public static void Refresh() => FusionCacheFactory.RemoveByTags(Tags);

    /// <summary>
    /// Cache key for getting all shifts with pagination.
    /// </summary>
    public const string GetAllCacheKey = "all-shifts";

    /// <summary>
    /// Cache key pattern for getting a specific shift by ID.
    /// </summary>
    /// <param name="id">The shift identifier.</param>
    /// <returns>The formatted cache key.</returns>
    public static string GetByIdCacheKey(int id) => $"shift-{id}";

    /// <summary>
    /// Cache tags for shift-related cache entries.
    /// </summary>
    public static IEnumerable<string> Tags => new[] { "Shifts" };
}
