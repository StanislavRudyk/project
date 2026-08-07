using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.UserName)
            .HasConversion(
                x => x.Value,
                x => UserName.Create(x))
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.UserName)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .HasConversion(
                x => x.Value,
                x => PasswordHash.Create(x))
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasConversion(
                x => x.HasValue ? x.Value.Value : null,
                x => x == null ? (Email?)null : Email.Create(x))
            .HasMaxLength(255)
            .IsRequired(false);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}