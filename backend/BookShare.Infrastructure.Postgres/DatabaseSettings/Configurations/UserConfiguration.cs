using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .IsRequired();
        
        builder.Property(x => x.UserName)
            .HasConversion(
                uname => uname.Value,
                value => UserName.Create(value))
            .IsRequired();
        
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.CreateAt).IsRequired();
    }
}