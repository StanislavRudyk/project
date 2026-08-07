using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using BookShare.Infrastructure.Postgres.DatabaseSettings;
using Microsoft.EntityFrameworkCore;

namespace BookShare.Infrastructure.Postgres.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    
    public UserRepository(DataContext context)
    {
        _dataContext = context;
    }
    public Task<User?> FindByIdAsync(Guid userId)
    {
        return _dataContext.Users.FindAsync(userId).AsTask();
    }

    public Task<User?> FindByEmailAsync(Email email)
    {
        return _dataContext.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task<User?> FindByUserNameAsync(UserName userName)
    {
        return _dataContext.Users
            .FirstOrDefaultAsync(x => x.UserName == userName);
    }

    public async Task<User?> FindByUserNameOrEmailAsync(string value)
    {
        if (value.Contains('@'))
        {
            return await _dataContext.Users
                .FirstOrDefaultAsync(x => x.Email == Email.Create(value));
        }

        return await _dataContext.Users
            .FirstOrDefaultAsync(x => x.UserName == UserName.Create(value));
    }

    public Task CreateAsync(User user)
    {
        return _dataContext.Users.AddAsync(user).AsTask();
    }

    public Task UpdateAsync(User user)
    {
        _dataContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        _dataContext.Users.Remove(user);
        return Task.CompletedTask;
    }
}