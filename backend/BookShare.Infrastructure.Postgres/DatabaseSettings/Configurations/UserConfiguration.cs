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
        
        builder.Property(x => x.Email)
            .HasConversion(
                email => email.HasValue ? email.Value.Value : null,
                value => value == null ? (Email?)null : Email.Create(value))
            .IsRequired(false);
        
        builder.Property(x => x.UserName)
            .HasConversion(
                uname => uname.Value,
                value => UserName.Create(value))
            .IsRequired();
        
        builder.Property(x => x.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => PasswordHash.Create(value))
            .IsRequired();
        
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}