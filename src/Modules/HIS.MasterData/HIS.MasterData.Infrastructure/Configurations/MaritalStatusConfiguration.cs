using HIS.MasterData.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HIS.MasterData.Infrastructure.Configurations;

public class MaritalStatusConfiguration : IEntityTypeConfiguration<MaritalStatus>
{
    public void Configure(EntityTypeBuilder<MaritalStatus> builder)
    {
        builder.ToTable("MaritalStatuses");
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.NameArabic).HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");
    }
}
