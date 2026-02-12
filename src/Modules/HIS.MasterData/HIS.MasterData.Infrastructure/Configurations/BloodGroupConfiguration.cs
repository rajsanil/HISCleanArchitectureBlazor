using HIS.MasterData.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HIS.MasterData.Infrastructure.Configurations;

public class BloodGroupConfiguration : IEntityTypeConfiguration<BloodGroup>
{
    public void Configure(EntityTypeBuilder<BloodGroup> builder)
    {
        builder.ToTable("BloodGroups");
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.NameArabic).HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_BloodGroups_Code")
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IX_BloodGroups_Name")
            .HasFilter("[Deleted] IS NULL");
    }
}
