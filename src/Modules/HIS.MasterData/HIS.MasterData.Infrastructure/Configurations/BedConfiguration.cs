using HIS.MasterData.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HIS.MasterData.Infrastructure.Configurations;

public class BedConfiguration : IEntityTypeConfiguration<Bed>
{
    public void Configure(EntityTypeBuilder<Bed> builder)
    {
        builder.ToTable("Beds");
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.Property(x => x.BedStatus).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasOne(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.Code, x.RoomId })
            .IsUnique()
            .HasDatabaseName("IX_Beds_Code_RoomId")
            .HasFilter("[Deleted] IS NULL");
    }
}
