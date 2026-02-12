// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Shifts.DTOs;
using CleanArchitecture.Blazor.Application.Features.Shifts.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Queries.GetAll;

/// <summary>
/// Query to get all active shifts.
/// </summary>
public class GetAllShiftsQuery : ICacheableRequest<IEnumerable<ShiftDto>>
{
    /// <summary>
    /// Gets the cache key.
    /// </summary>
    public string CacheKey => ShiftCacheKey.GetAllCacheKey;

    /// <summary>
    /// Gets the cache tags.
    /// </summary>
    public IEnumerable<string>? Tags => ShiftCacheKey.Tags;

    /// <summary>
    /// Gets the cache expiration time.
    /// </summary>
    public TimeSpan? Expiration => TimeSpan.FromHours(1);
}

/// <summary>
/// Handler for GetAllShiftsQuery.
/// </summary>
public class GetAllShiftsQueryHandler : IRequestHandler<GetAllShiftsQuery, IEnumerable<ShiftDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllShiftsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShiftDto>> Handle(GetAllShiftsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Shifts
            .Where(x => x.IsActive)
            .OrderBy(x => x.FromTime)
            .Select(x => new ShiftDto
            {
                Id = x.Id,
                ShiftCode = x.Code,
                ShiftName = x.Name,
                FromTime = x.FromTime,
                ToTime = x.ToTime,
                IsActive = x.IsActive,
                TenantId = x.TenantId,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy
            })
            .ToListAsync(cancellationToken);
    }
}
