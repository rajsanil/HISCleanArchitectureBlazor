// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.UserFavorites.DTOs;

public class UserFavoriteDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public FavoriteType Type { get; set; }
    public string ItemKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public DateTime? Created { get; set; }
}
