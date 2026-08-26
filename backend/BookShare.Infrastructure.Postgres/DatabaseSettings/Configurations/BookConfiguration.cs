using BookShare.Domain.Models;
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
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .HasMaxLength(5000);

        builder.Property(x => x.Author)
            .HasMaxLength(300);

        builder.Property(x => x.CoverKey)
            .HasMaxLength(500);

        builder.Property(x => x.FileKey)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.FileHash)
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