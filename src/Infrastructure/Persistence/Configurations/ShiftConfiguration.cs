// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Shift.
/// </summary>
public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        // Table configuration
        builder.ToTable("Shifts");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Code)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.FromTime)
            .IsRequired();

        builder.Property(x => x.ToTime)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.TenantId)
            .IsRequired()
            .HasMaxLength(450);

        // Indexes with soft delete filter
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[Deleted] IS NULL");

        builder.HasIndex(x => x.TenantId)
            .HasDatabaseName("IX_Shifts_TenantId");

        // Audit fields are handled by BaseAuditableEntity interceptor
    }
}
