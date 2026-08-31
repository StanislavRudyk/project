using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookShare.Domain.Models;

namespace BookShare.Infrastructure.Postgres.Configurations;

public sealed class WorkConfiguration : IEntityTypeConfiguration<Work>{
    public void Configure(EntityTypeBuilder<Work> builder) {
        builder.ToTable("works");
        builder.HasKey(w => w.Identifier);

        builder.Property(w => w.OriginalTitle).HasMaxLength(500).IsRequired();
        builder.Property(w => w.Author).HasMaxLength(300).IsRequired();
        builder.Property(w => w.Genres).HasMaxLength(500);

        builder.HasMany(w => w.Editions)
               .WithOne(e => e.Work)
               .HasForeignKey(e => e.WorkIdentifier)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
