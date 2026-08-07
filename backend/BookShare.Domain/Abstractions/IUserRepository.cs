using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Abstractions;

public interface IUserRepository
{
    // All
    Task<User?> FindByIdAsync(Guid userId);
    
    // Search in site
    Task<User?> FindByEmailAsync(Email email);
    Task<User?> FindByUserNameAsync(UserName userName);
    
    // Login
    Task<User?> FindByUserNameOrEmailAsync(string value);
    
    // CRUD
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    
}