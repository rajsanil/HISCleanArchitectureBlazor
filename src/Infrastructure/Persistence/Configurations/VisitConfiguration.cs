using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.VisitNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.VisitType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.VisitStatus).HasMaxLength(20).IsRequired().HasDefaultValue("Registered");
        builder.Property(x => x.DischargeDisposition).HasMaxLength(50);
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();

        builder.HasIndex(x => x.VisitNumber)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");
        builder.HasIndex(x => new { x.PatientId, x.VisitStatus });

        builder.HasOne(x => x.Patient)
            .WithMany(x => x.Visits)
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Facility)
            .WithMany()
            .HasForeignKey(x => x.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AttendingDoctor)
            .WithMany()
            .HasForeignKey(x => x.AttendingDoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
