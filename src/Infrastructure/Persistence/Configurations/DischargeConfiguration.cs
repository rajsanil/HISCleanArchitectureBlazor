using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class DischargeConfiguration : IEntityTypeConfiguration<Discharge>
{
    public void Configure(EntityTypeBuilder<Discharge> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.DischargeDisposition).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FollowUpNotes).HasMaxLength(500);
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();

        builder.HasIndex(x => x.VisitId).IsUnique(); // one-to-one with Visit

        builder.HasOne(x => x.Visit)
            .WithOne(x => x.Discharge)
            .HasForeignKey<Discharge>(x => x.VisitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DischargedByDoctor)
            .WithMany()
            .HasForeignKey(x => x.DischargedByDoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
