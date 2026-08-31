using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookShare.Domain.Models;

namespace BookShare.Infrastructure.Postgres.Configurations;

public sealed class UserLibraryConfiguration : IEntityTypeConfiguration<UserLibrary>
{
    public void Configure(EntityTypeBuilder<UserLibrary> builder)
    {
        builder.ToTable("user_libraries");

        builder.HasKey(ul => ul.Identifier);

        builder.HasOne(ul => ul.FileAsset)
               .WithMany()
               .HasForeignKey(ul => ul.FileAssetHash)
               .OnDelete(DeleteBehavior.Cascade);
    }
}