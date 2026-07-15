using BookShare.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings;

public sealed class DataContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}