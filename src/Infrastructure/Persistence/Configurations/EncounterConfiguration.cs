using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class EncounterConfiguration : IEntityTypeConfiguration<Encounter>
{
    public void Configure(EntityTypeBuilder<Encounter> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.EncounterNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.EncounterType).HasMaxLength(30).IsRequired();
        builder.Property(x => x.EncounterStatus).HasMaxLength(20).IsRequired().HasDefaultValue("Planned");
        builder.Property(x => x.ChiefComplaint).HasMaxLength(500);
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();

        builder.HasIndex(x => x.EncounterNumber)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasOne(x => x.Visit)
            .WithMany(x => x.Encounters)
            .HasForeignKey(x => x.VisitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Doctor)
            .WithMany()
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
