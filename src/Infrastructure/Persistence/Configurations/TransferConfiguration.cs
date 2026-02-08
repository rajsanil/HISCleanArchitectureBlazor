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

        builder.HasOne(x => x.FromBed)
            .WithMany()
            .HasForeignKey(x => x.FromBedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToBed)
            .WithMany()
            .HasForeignKey(x => x.ToBedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FromLocation)
            .WithMany()
            .HasForeignKey(x => x.FromLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToLocation)
            .WithMany()
            .HasForeignKey(x => x.ToLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OrderedByDoctor)
            .WithMany()
            .HasForeignKey(x => x.OrderedByDoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
