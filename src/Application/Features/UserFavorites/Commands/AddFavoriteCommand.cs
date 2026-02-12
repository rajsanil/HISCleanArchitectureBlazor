// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.UserFavorites.Caching;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace CleanArchitecture.Blazor.Application.Features.UserFavorites.Commands;

public class AddFavoriteCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string UserId { get; set; } = string.Empty;
    public FavoriteType Type { get; set; }
    public string ItemKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }

    public string CacheKey => UserFavoriteCacheKey.GetCacheKey(UserId);
    public IEnumerable<string>? Tags => UserFavoriteCacheKey.Tags;
}

public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFusionCache _fusionCache;

    public AddFavoriteCommandHandler(IApplicationDbContext context, IFusionCache fusionCache)
    {
        _context = context;
        _fusionCache = fusionCache;
    }

    public async Task<Result<int>> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        // Check if already exists
        var existing = await _context.UserFavorites
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.ItemKey == request.ItemKey, cancellationToken);

        if (existing != null)
            return await Result<int>.FailureAsync("This item is already in your favorites.");

        var favorite = new UserFavorite
        {
            UserId = request.UserId,
            Type = request.Type,
            ItemKey = request.ItemKey,
            Title = request.Title,
            Icon = request.Icon,
            Description = request.Description
        };

        favorite.AddDomainEvent(new CreatedEvent<UserFavorite>(favorite));
        _context.UserFavorites.Add(favorite);
        await _context.SaveChangesAsync(cancellationToken);

        // Explicitly remove cache to ensure immediate invalidation
        var cacheKey = UserFavoriteCacheKey.GetCacheKey(request.UserId);
        _fusionCache.Remove(cacheKey);
        await _fusionCache.RemoveByTagAsync("UserFavorites");

        return await Result<int>.SuccessAsync(favorite.Id);
    }
}
