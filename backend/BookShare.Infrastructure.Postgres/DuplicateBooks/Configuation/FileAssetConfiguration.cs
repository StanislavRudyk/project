using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookShare.Domain.Models;

namespace BookShare.Infrastructure.Postgres.Configurations;

public sealed class FileAssetConfiguration : IEntityTypeConfiguration<FileAsset>
{
    public void Configure(EntityTypeBuilder<FileAsset> builder)
    {
        builder.ToTable("file_assets");
        
        // SHA-256 (Primary Key)
        builder.HasKey(f => f.Hash);
        builder.Property(f => f.Hash)
               .HasColumnType("char(64)")
               .ValueGeneratedNever();

        builder.Property(f => f.ContentHash)
               .HasMaxLength(256);

        builder.Property(f => f.MimeType)
               .HasMaxLength(100)
               .IsRequired();
    }
}