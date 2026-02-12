// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Shifts.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Queries.Pagination;

/// <summary>
/// Query to get shifts with pagination.
/// </summary>
public class ShiftsWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<ShiftDto>>
{
    /// <summary>
    /// Gets or sets the list view filter.
    /// </summary>
    public ShiftListView ListView { get; set; } = ShiftListView.All;

    /// <summary>
    /// Gets the cache key.
    /// </summary>
    public string CacheKey => $"ShiftsWithPagination,{PageNumber},{PageSize},{OrderBy},{SortDirection},{Keyword},{ListView}";

    /// <summary>
    /// Gets the cache tags.
    /// </summary>
    public IEnumerable<string>? Tags => new[] { "Shifts" };

    /// <summary>
    /// Gets the cache expiration time.
    /// </summary>
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}

/// <summary>
/// List view filter for shifts.
/// </summary>
public enum ShiftListView
{
    All,
    Active,
    Inactive
}
