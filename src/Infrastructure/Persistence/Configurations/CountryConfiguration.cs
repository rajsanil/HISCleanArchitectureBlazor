using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameArabic).HasMaxLength(200);
        builder.Property(x => x.Iso2Code).HasMaxLength(2);
        builder.Property(x => x.Iso3Code).HasMaxLength(3);
        builder.Property(x => x.PhoneCode).HasMaxLength(10);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.Iso2Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL AND [Iso2Code] IS NOT NULL");

        builder.HasIndex(x => x.Iso3Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL AND [Iso3Code] IS NOT NULL");
    }
}
