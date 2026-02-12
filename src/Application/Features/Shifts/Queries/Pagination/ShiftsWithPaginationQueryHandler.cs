// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Shifts.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Shifts.Queries.Pagination;

/// <summary>
/// Handler for ShiftsWithPaginationQuery.
/// </summary>
public class ShiftsWithPaginationQueryHandler : IRequestHandler<ShiftsWithPaginationQuery, PaginatedData<ShiftDto>>
{
    private readonly IApplicationDbContext _context;

    public ShiftsWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedData<ShiftDto>> Handle(ShiftsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Shifts.AsQueryable();

        // Apply ListView filter
        query = request.ListView switch
        {
            ShiftListView.Active => query.Where(x => x.IsActive),
            ShiftListView.Inactive => query.Where(x => !x.IsActive),
            _ => query
        };

        // Apply keyword search
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x => 
                x.Code.Contains(request.Keyword) || 
                x.Name.Contains(request.Keyword));
        }

        // Apply ordering
        query = request.OrderBy switch
        {
            "Code" => request.SortDirection == "Descending" 
                ? query.OrderByDescending(x => x.Code) 
                : query.OrderBy(x => x.Code),
            "Name" => request.SortDirection == "Descending" 
                ? query.OrderByDescending(x => x.Name) 
                : query.OrderBy(x => x.Name),
            "FromTime" => request.SortDirection == "Descending" 
                ? query.OrderByDescending(x => x.FromTime) 
                : query.OrderBy(x => x.FromTime),
            _ => query.OrderBy(x => x.Code)
        };

        // Apply pagination and projection
        var projectedQuery = query.Select(x => new ShiftDto
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
            });

        return await PaginatedData<ShiftDto>.CreateAsync(projectedQuery, request.PageNumber, request.PageSize);
    }
}
