// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Shifts.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Commands.Delete;

/// <summary>
/// Command to delete a shift.
/// </summary>
public class DeleteShiftCommand : ICacheInvalidatorRequest<Result<int>>
{
    /// <summary>
    /// Gets or sets the shift identifiers to delete.
    /// </summary>
    public int[] Id { get; set; } = Array.Empty<int>();

    /// <summary>
    /// Gets the cache key for invalidation.
    /// </summary>
    public string CacheKey => ShiftCacheKey.GetAllCacheKey;

    /// <summary>
    /// Gets the cache tags for invalidation.
    /// </summary>
    public IEnumerable<string>? Tags => ShiftCacheKey.Tags;
}
