using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
                value => Domain.ValueObject.BookTitle.Create(value))
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .HasMaxLength(5000);

        builder.Property(x => x.Author)
            .HasMaxLength(300);

        var coverKeyConverter = new ValueConverter<CoverKey?, string?>(
            coverKey => coverKey.HasValue
                ? coverKey.Value.Value
                : null,
            value => value == null
                ? null
                : CoverKey.Create(value));
        
        builder.Property(x => x.CoverKey)
            .HasConversion(coverKeyConverter)
            .HasMaxLength(500);

        builder.Property(x => x.FileKey)
            .HasConversion(
                fileKey => fileKey.Value,
                value => Domain.ValueObject.FileKey.Create(value))
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.FileHash)
            .HasConversion(
                fileHash => fileHash.Value,
                value => Domain.ValueObject.FileHash.Create(value))
            .IsRequired()
            .HasMaxLength(64);

        builder.HasIndex(x => x.FileHash)
            .IsUnique();

        builder.Property(x => x.FileSize)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UploadedById)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}