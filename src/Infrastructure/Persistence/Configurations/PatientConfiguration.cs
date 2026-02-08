using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(x => x.FullName);

        builder.Property(x => x.MRN).HasMaxLength(20).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.MiddleName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FirstNameArabic).HasMaxLength(100);
        builder.Property(x => x.LastNameArabic).HasMaxLength(100);
        builder.Property(x => x.Gender).HasMaxLength(20).IsRequired();
        builder.Property(x => x.EmiratesId).HasMaxLength(20);
        builder.Property(x => x.PassportNumber).HasMaxLength(20);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.PhotoUrl).HasMaxLength(500);
        builder.Property(x => x.TenantId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasOne(x => x.Nationality)
            .WithMany()
            .HasForeignKey(x => x.NationalityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.BloodGroup)
            .WithMany()
            .HasForeignKey(x => x.BloodGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MaritalStatus)
            .WithMany()
            .HasForeignKey(x => x.MaritalStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.MRN)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");
        builder.HasIndex(x => x.EmiratesId);
        builder.HasIndex(x => x.Phone);
        builder.HasIndex(x => new { x.LastName, x.FirstName });
    }
}
