// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Logger> Loggers { get; set; }
    DbSet<AuditTrail> AuditTrails { get; set; }
    DbSet<Document> Documents { get; set; }
    DbSet<PicklistSet> PicklistSets { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Tenant> Tenants { get; set; }
    ChangeTracker ChangeTracker { get; }

    DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    // HIS - Foundation (moved to HIS.MasterData module)
    DbSet<Facility> Facilities { get; set; }
    DbSet<Room> Rooms { get; set; }
    DbSet<Staff> Staff { get; set; }

    // HIS - Patient
    DbSet<Patient> Patients { get; set; }
    DbSet<PatientContact> PatientContacts { get; set; }

    // HIS - Visit & Encounter
    DbSet<Visit> Visits { get; set; }
    DbSet<Encounter> Encounters { get; set; }
    DbSet<Admission> Admissions { get; set; }
    DbSet<Transfer> Transfers { get; set; }
    DbSet<Discharge> Discharges { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}