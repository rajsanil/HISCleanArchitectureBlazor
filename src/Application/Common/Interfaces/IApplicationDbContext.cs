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
    DbSet<Contact> Contacts { get; set; }
    ChangeTracker ChangeTracker { get; }

    DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    // HIS - Master/Lookup Tables
    DbSet<BloodGroup> BloodGroups { get; set; }
    DbSet<MaritalStatus> MaritalStatuses { get; set; }
    DbSet<City> Cities { get; set; }
    DbSet<Country> Countries { get; set; }
    DbSet<Nationality> Nationalities { get; set; }

    // HIS - Foundation
    DbSet<Facility> Facilities { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<Specialty> Specialties { get; set; }
    DbSet<Location> Locations { get; set; }
    DbSet<Room> Rooms { get; set; }
    DbSet<Bed> Beds { get; set; }
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