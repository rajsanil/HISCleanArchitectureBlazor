using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
{
    public void Configure(EntityTypeBuilder<Admission> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.AdmissionType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();

        builder.HasIndex(x => x.VisitId)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL"); // one-to-one with Visit

        builder.HasOne(x => x.Visit)
            .WithOne(x => x.Admission)
            .HasForeignKey<Admission>(x => x.VisitId)
            .OnDelete(DeleteBehavior.Restrict);

        // Note: Bed navigation removed - entity moved to MasterData module
        // Foreign key relationship maintained via BedId

        builder.HasOne(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Note: Location navigation removed - entity moved to MasterData module
        // Foreign key relationship maintained via LocationId

        builder.HasOne(x => x.AdmittingDoctor)
            .WithMany()
            .HasForeignKey(x => x.AdmittingDoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
