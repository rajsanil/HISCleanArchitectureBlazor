// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

/// <summary>
/// Represents a work shift in the system.
/// </summary>
public class Shift : BaseAuditableSoftDeleteEntity
{
    /// <summary>
    /// Unique code for the shift.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the shift.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Start time of the shift.
    /// </summary>
    public TimeSpan FromTime { get; set; }

    /// <summary>
    /// End time of the shift.
    /// </summary>
    public TimeSpan ToTime { get; set; }

    /// <summary>
    /// Indicates whether the shift is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Multi-tenancy support.
    /// </summary>
    public string? TenantId { get; set; }
}
