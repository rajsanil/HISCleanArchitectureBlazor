// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.UserFavorites.DTOs;
using CleanArchitecture.Blazor.Application.Features.UserFavorites.Caching;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Blazor.Application.Features.UserFavorites.Queries;

public class GetUserFavoritesQuery : ICacheableRequest<IEnumerable<UserFavoriteDto>>
{
    public string UserId { get; set; } = string.Empty;
    
    public string CacheKey => UserFavoriteCacheKey.GetCacheKey(UserId);
    public IEnumerable<string>? Tags => UserFavoriteCacheKey.Tags;
}

public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, IEnumerable<UserFavoriteDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUserFavoritesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserFavoriteDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favorites = await _context.UserFavorites
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.Created)
            .Select(x => new UserFavoriteDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Type = x.Type,
                ItemKey = x.ItemKey,
                Title = x.Title,
                Icon = x.Icon,
                Description = x.Description,
                Created = x.Created
            })
            .ToListAsync(cancellationToken);

        return favorites;
    }
}
