using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Infrastructure.Postgres.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookShare.Infrastructure.Security;

public class AccessTokenGenerator : IAccessTokenGenerator
{
    private readonly JwtOptions _options;

    public AccessTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public string Generate(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName.Value),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.SecretKey));
        
        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}