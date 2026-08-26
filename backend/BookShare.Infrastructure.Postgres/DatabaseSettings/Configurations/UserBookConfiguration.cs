using BookShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings.Configurations;

public sealed class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
{
    public void Configure(EntityTypeBuilder<UserBook> builder)
    {
        builder.ToTable("user_books");

        builder.HasKey(x => new
        {
            x.UserId,
            x.BookId
        });

        builder.Property(x => x.AddedAt)
            .IsRequired();

        builder.Property(x => x.IsFavorite)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Book)
            .WithMany()
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}