using BookShare.Domain.Abstractions;

namespace BookShare.Infrastructure.Postgres.DatabaseSettings;

// Обёртка над EF Core
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    
    public UnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dataContext.SaveChangesAsync(cancellationToken);
    }
}