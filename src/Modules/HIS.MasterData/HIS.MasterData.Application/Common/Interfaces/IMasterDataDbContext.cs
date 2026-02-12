namespace HIS.MasterData.Application.Common.Interfaces;

/// <summary>
/// Application database context interface for MasterData module.
/// Provides access to module-specific entity sets.
/// </summary>
public interface IMasterDataDbContext
{
    DbSet<Bed> Beds { get; set; }
    DbSet<BloodGroup> BloodGroups { get; set; }
    DbSet<City> Cities { get; set; }
    DbSet<Contact> Contacts { get; set; }
    DbSet<Country> Countries { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<Location> Locations { get; set; }
    DbSet<MaritalStatus> MaritalStatuses { get; set; }
    DbSet<Nationality> Nationalities { get; set; }
    DbSet<Specialty> Specialties { get; set; }

    Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker ChangeTracker { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
