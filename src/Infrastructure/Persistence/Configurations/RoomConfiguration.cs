using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.RoomType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        // Note: Location navigation removed - entity moved to MasterData module
        // Foreign key relationship maintained via LocationId

        builder.HasIndex(x => new { x.Code, x.LocationId })
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");
    }
}
