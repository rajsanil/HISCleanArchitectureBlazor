// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;

/// <summary>
/// Represents a user's favorite item (menu, page, or module).
/// </summary>
public class UserFavorite : BaseAuditableEntity, IMayHaveTenant
{
    /// <summary>
    /// The user who favorited this item.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// The type of favorite (Menu, Module, Page, etc.).
    /// </summary>
    public FavoriteType Type { get; set; }
    
    /// <summary>
    /// The unique key identifying the favorited item (e.g., URL path, module ID).
    /// </summary>
    public string ItemKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Display title for the favorite.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional icon for the favorite.
    /// </summary>
    public string? Icon { get; set; }
    
    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Multi-tenancy support.
    /// </summary>
    public string? TenantId { get; set; }
    
    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public virtual ApplicationUser? User { get; set; }
    
    /// <summary>
    /// Navigation property to the tenant.
    /// </summary>
    public virtual Tenant? Tenant { get; set; }
}

/// <summary>
/// Types of items that can be favorited.
/// </summary>
public enum FavoriteType
{
    /// <summary>
    /// A menu item or navigation link.
    /// </summary>
    MenuItem,
    
    /// <summary>
    /// A module or feature.
    /// </summary>
    Module,
    
    /// <summary>
    /// A specific page.
    /// </summary>
    Page,
    
    /// <summary>
    /// A dashboard widget.
    /// </summary>
    Widget
}
