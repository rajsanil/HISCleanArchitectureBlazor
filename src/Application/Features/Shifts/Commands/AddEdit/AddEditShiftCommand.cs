// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Shifts.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Commands.AddEdit;

/// <summary>
/// Command to add or edit a shift.
/// </summary>
public class AddEditShiftCommand : ICacheInvalidatorRequest<Result<int>>
{
    /// <summary>
    /// Gets or sets the shift identifier. 0 for new shifts.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique code for the shift.
    /// </summary>
    public string ShiftCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the shift.
    /// </summary>
    public string ShiftName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start time of the shift.
    /// </summary>
    public TimeSpan FromTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the shift.
    /// </summary>
    public TimeSpan ToTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the shift is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets the cache key for invalidation.
    /// </summary>
    public string CacheKey => ShiftCacheKey.GetAllCacheKey;

    /// <summary>
    /// Gets the cache tags for invalidation.
    /// </summary>
    public IEnumerable<string>? Tags => ShiftCacheKey.Tags;
}
