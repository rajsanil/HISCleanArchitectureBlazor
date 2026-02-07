// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Domain.Common.Entities;

/// <summary>
/// Base auditable soft-delete entity class with string primary key support.
/// Extends BaseAuditableEntityWithStringKey with soft delete capabilities.
/// </summary>
public abstract class BaseAuditableSoftDeleteEntityWithStringKey : BaseAuditableEntityWithStringKey, ISoftDelete
{
    /// <summary>
    /// Gets or sets the date and time when the entity was soft-deleted.
    /// Null indicates the entity is not deleted.
    /// </summary>
    public DateTime? Deleted { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who deleted the entity.
    /// </summary>
    public string? DeletedBy { get; set; }
}
