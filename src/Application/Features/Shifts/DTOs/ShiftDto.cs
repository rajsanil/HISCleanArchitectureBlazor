// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Shifts.DTOs;

/// <summary>
/// Data Transfer Object for Shift entity.
/// </summary>
public class ShiftDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the shift.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique code for the shift.
    /// </summary>
    public string ShiftCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the shift.
    /// </summary>
    public string ShiftName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start time of the shift.
    /// </summary>
    public TimeSpan FromTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the shift.
    /// </summary>
    public TimeSpan ToTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the shift is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the tenant identifier.
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the shift was created.
    /// </summary>
    public DateTime? Created { get; set; }

    /// <summary>
    /// Gets or sets the user who created the shift.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the shift was last modified.
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// Gets or sets the user who last modified the shift.
    /// </summary>
    public string? LastModifiedBy { get; set; }
}
