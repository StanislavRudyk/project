using BookShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShare.Infrastructure.Postgres.Configurations;

public sealed class EditionConfiguration : IEntityTypeConfiguration<Edition>
{
    public void Configure(EntityTypeBuilder<Edition> builder)
    {
        builder.ToTable("editions");
        builder.HasKey(e => e.Identifier);

        builder.Property(e => e.Language).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Translator).HasMaxLength(300);
        builder.Property(e => e.Publisher).HasMaxLength(300);

        builder.HasMany(e => e.FileAssets)
               .WithOne(f => f.Edition)
               .HasForeignKey(f => f.EditionIdentifier)
               .OnDelete(DeleteBehavior.Restrict);
    }
}