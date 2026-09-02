using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings.Configurations;

public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasConversion(
                title => title.Value,
                value => BookTitle.Create(value))
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .HasMaxLength(5000);

        builder.Property(x => x.Author)
            .HasMaxLength(300);

        builder.Property(x => x.CoverKey)
            .HasConversion(
                coverKey => coverKey.HasValue
                    ? coverKey.Value.Value
                    : null,
                value => value == null
                    ? null
                    : CoverKey.Create(value))
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UploadedById)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Files)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}