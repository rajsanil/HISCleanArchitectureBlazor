// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class PicklistSet : BaseAuditableEntity, IAuditTrial
{
    public Picklist Name { get; set; } = Picklist.Brand;
    public string? Value { get; set; }
    public string? Text { get; set; }
    public string? Description { get; set; }
}

public enum Picklist
{
    [Description("Status")] Status,
    [Description("Unit")] Unit,
    [Description("Brand")] Brand,

    // HIS - Demographics
    [Description("Gender")] Gender,
    [Description("Marital Status")] MaritalStatus,
    [Description("Blood Group")] BloodGroup,
    [Description("Nationality")] Nationality,
    [Description("Relationship")] Relationship,

    // HIS - Visit & Encounter
    [Description("Visit Type")] VisitType,
    [Description("Visit Status")] VisitStatus,
    [Description("Encounter Type")] EncounterType,
    [Description("Encounter Status")] EncounterStatus,
    [Description("Admission Type")] AdmissionType,
    [Description("Discharge Disposition")] DischargeDisposition,

    // HIS - Facility & Beds
    [Description("Location Type")] LocationType,
    [Description("Room Type")] RoomType,
    [Description("Bed Status")] BedStatus,
    [Description("Staff Type")] StaffType,

    // HIS - Orders (future phases)
    [Description("Order Status")] OrderStatus,
    [Description("Order Priority")] OrderPriority,
    [Description("Order Type")] OrderType
}