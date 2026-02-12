using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Reason).HasMaxLength(500);
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();

        builder.HasOne(x => x.Visit)
            .WithMany(x => x.Transfers)
            .HasForeignKey(x => x.VisitId)
            .OnDelete(DeleteBehavior.Restrict);

        // Note: Bed and Location navigations removed - entities moved to MasterData module
        // Foreign key relationships maintained via FromBedId, ToBedId, FromLocationId, ToLocationId

        builder.HasOne(x => x.OrderedByDoctor)
            .WithMany()
            .HasForeignKey(x => x.OrderedByDoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
