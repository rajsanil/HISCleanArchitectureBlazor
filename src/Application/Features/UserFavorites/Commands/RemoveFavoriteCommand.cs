// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.UserFavorites.Caching;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace CleanArchitecture.Blazor.Application.Features.UserFavorites.Commands;

public class RemoveFavoriteCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string UserId { get; set; } = string.Empty;
    public string ItemKey { get; set; } = string.Empty;

    public string CacheKey => UserFavoriteCacheKey.GetCacheKey(UserId);
    public IEnumerable<string>? Tags => UserFavoriteCacheKey.Tags;
}

public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFusionCache _fusionCache;

    public RemoveFavoriteCommandHandler(IApplicationDbContext context, IFusionCache fusionCache)
    {
        _context = context;
        _fusionCache = fusionCache;
    }

    public async Task<Result<int>> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
    {
        var favorite = await _context.UserFavorites
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.ItemKey == request.ItemKey, cancellationToken);

        if (favorite == null)
            return await Result<int>.FailureAsync("Favorite not found.");

        favorite.AddDomainEvent(new DeletedEvent<UserFavorite>(favorite));
        _context.UserFavorites.Remove(favorite);
        await _context.SaveChangesAsync(cancellationToken);

        // Explicitly remove cache to ensure immediate invalidation
        var cacheKey = UserFavoriteCacheKey.GetCacheKey(request.UserId);
        _fusionCache.Remove(cacheKey);
        await _fusionCache.RemoveByTagAsync("UserFavorites");

        return await Result<int>.SuccessAsync(favorite.Id);
    }
}
