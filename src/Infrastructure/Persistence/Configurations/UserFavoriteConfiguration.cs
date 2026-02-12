// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

public class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
{
    public void Configure(EntityTypeBuilder<UserFavorite> builder)
    {
        builder.ToTable("UserFavorites");

        builder.Property(x => x.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.ItemKey)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Icon)
            .HasMaxLength(2000);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        // Create unique index on UserId + ItemKey to prevent duplicates
        builder.HasIndex(x => new { x.UserId, x.ItemKey })
            .IsUnique()
            .HasDatabaseName("IX_UserFavorites_UserId_ItemKey");

        // Create index on UserId for faster queries
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_UserFavorites_UserId");

        // Relationship with User
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship with Tenant (optional)
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
