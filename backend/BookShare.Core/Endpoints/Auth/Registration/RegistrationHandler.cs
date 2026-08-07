using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;

namespace BookShare.Core.Endpoints.Auth.Registration;

public sealed class RegistrationHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public RegistrationHandler(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = users;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(RegistrationRequest request)
    {
        var userName = UserName.Create(request.Username);

        Email? email = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
            email = Email.Create(request.Email);

        if (await _userRepository.FindByUserNameAsync(userName) is not null)
            throw new InvalidOperationException("Username already exists.");

        if (email is not null &&
            await _userRepository.FindByEmailAsync(email.Value) is not null)
            throw new InvalidOperationException("Email already exists.");

        var passwordHash = _passwordHasher.GenerateHash(request.Password);

        var user = User.Create(
            userName,
            passwordHash,
            email);

        await _userRepository.CreateAsync(user);

        await _unitOfWork.SaveChangesAsync();

        return user.Id;
    }
}