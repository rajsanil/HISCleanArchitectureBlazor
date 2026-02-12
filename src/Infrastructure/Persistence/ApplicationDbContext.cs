// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Identity;
using HIS.Core.Abstractions;
using HIS.MasterData.Application.Common.Interfaces;
using HIS.MasterData.Domain.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence;

#nullable disable
public class ApplicationDbContext : IdentityDbContext<
    ApplicationUser, ApplicationRole, string,
    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
    ApplicationRoleClaim, ApplicationUserToken>, IApplicationDbContext, IMasterDataDbContext, IDataProtectionKeyContext
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Logger> Loggers { get; set; }
    public DbSet<AuditTrail> AuditTrails { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public DbSet<PicklistSet> PicklistSets { get; set; }
    
    // HIS - Foundation (Module entities via explicit interface implementation)
    DbSet<HIS.MasterData.Domain.Entities.Bed> IMasterDataDbContext.Beds { get => Set<HIS.MasterData.Domain.Entities.Bed>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.BloodGroup> IMasterDataDbContext.BloodGroups { get => Set<HIS.MasterData.Domain.Entities.BloodGroup>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.City> IMasterDataDbContext.Cities { get => Set<HIS.MasterData.Domain.Entities.City>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.Contact> IMasterDataDbContext.Contacts { get => Set<HIS.MasterData.Domain.Entities.Contact>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.Country> IMasterDataDbContext.Countries { get => Set<HIS.MasterData.Domain.Entities.Country>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.Department> IMasterDataDbContext.Departments { get => Set<HIS.MasterData.Domain.Entities.Department>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.Location> IMasterDataDbContext.Locations { get => Set<HIS.MasterData.Domain.Entities.Location>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.MaritalStatus> IMasterDataDbContext.MaritalStatuses { get => Set<HIS.MasterData.Domain.Entities.MaritalStatus>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.Nationality> IMasterDataDbContext.Nationalities { get => Set<HIS.MasterData.Domain.Entities.Nationality>(); set { } }
    DbSet<HIS.MasterData.Domain.Entities.Specialty> IMasterDataDbContext.Specialties { get => Set<HIS.MasterData.Domain.Entities.Specialty>(); set { } }

    // Backward compatibility public accessors
    public DbSet<BloodGroup> BloodGroups => Set<BloodGroup>();
    public DbSet<MaritalStatus> MaritalStatuses => Set<MaritalStatus>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Nationality> Nationalities => Set<Nationality>();

    // HIS - Foundation (Core entities)
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Staff> Staff { get; set; }

    // HIS - Patient
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientContact> PatientContacts { get; set; }

    // HIS - Visit & Encounter
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Encounter> Encounters { get; set; }
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Discharge> Discharges { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply core platform configurations
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Apply module configurations dynamically
        try
        {
            var moduleLoader = _serviceProvider?.GetService<IModuleLoader>();
            if (moduleLoader != null)
            {
                var activeModules = moduleLoader.GetActiveModules();
                foreach (var module in activeModules)
                {
                    module.ConfigureDatabase(builder);
                }
            }
        }
        catch
        {
            // If module loader is not available (e.g., during design-time), continue without modules
            // This allows migrations to work even if the full DI container isn't initialized
        }
        
        // Global filters
        builder.ApplyGlobalFilters<ISoftDelete>(s => s.Deleted == null);
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>().HaveMaxLength(450);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Suppress pending model changes warning during development
        // Remove this after creating and applying migrations
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
}