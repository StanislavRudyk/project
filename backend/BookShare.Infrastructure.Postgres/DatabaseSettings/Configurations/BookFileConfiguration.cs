using BookShare.Domain.Enums;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings.Configurations;

public sealed class BookFileConfiguration
    : IEntityTypeConfiguration<BookFile>
{
    public void Configure(EntityTypeBuilder<BookFile> builder)
    {
        builder.ToTable("book_files");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BookId)
            .IsRequired();

        builder.Property(x => x.Format)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.FileKey)
            .HasConversion(
                fileKey => fileKey.Value,
                value => FileKey.Create(value))
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.FileHash)
            .HasConversion(
                fileHash => fileHash.Value,
                value => FileHash.Create(value))
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.FileSize)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.FileHash)
            .IsUnique();

        builder.HasIndex(x => new
            {
                x.BookId,
                x.Format
            })
            .IsUnique();
    }
}