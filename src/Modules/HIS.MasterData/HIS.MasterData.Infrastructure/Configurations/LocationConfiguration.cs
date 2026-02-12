using HIS.MasterData.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HIS.MasterData.Infrastructure.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations");
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.LocationType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasOne(x => x.Facility)
            .WithMany()
            .HasForeignKey(x => x.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.Code, x.FacilityId })
            .IsUnique()
            .HasDatabaseName("IX_Locations_Code_FacilityId")
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => new { x.Name, x.FacilityId })
            .IsUnique()
            .HasDatabaseName("IX_Locations_Name_FacilityId")
            .HasFilter("[Deleted] IS NULL");
    }
}
