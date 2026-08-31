using BookShare.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings;

public sealed class DataContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();

    public DbSet<Work> Works => Set<Work>();
    public DbSet<Edition> Editions => Set<Edition>();
    public DbSet<FileAsset> FileAssets => Set<FileAsset>();
    public DbSet<UserLibrary> UserLibraries => Set<UserLibrary>();
    
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}