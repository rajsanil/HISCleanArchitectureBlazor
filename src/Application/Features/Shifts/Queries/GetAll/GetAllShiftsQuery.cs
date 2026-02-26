// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Shifts.DTOs;
using CleanArchitecture.Blazor.Application.Features.Shifts.Caching;
using CleanArchitecture.Blazor.Application.Features.Shifts.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Queries.GetAll;

/// <summary>
/// Query to get all active shifts.
/// </summary>
public class GetAllShiftsQuery : ICacheableRequest<IEnumerable<ShiftDto>>
{
    public string CacheKey => ShiftCacheKey.GetAllCacheKey;
    public IEnumerable<string>? Tags => ShiftCacheKey.Tags;
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
            .ProjectTo()
            .ToListAsync(cancellationToken);
    }
}
