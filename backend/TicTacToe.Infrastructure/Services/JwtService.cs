using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;
using TicTacToe.Infrastructure.Options;

namespace TicTacToe.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly AuthOptions _options;
    
    public JwtService(IConfiguration configuration)
    {
        _options = configuration.GetSection("JwtSettings").Get<AuthOptions>()!;
    }
    public string GenerateToken(User user)
    {
        Claim[] claims = 
        {
            new ("Id", user.Id.ToString()),
            new ("Name", user.Name)
        };

        var issuer = _options.Issuer;
        var audience = _options.Audience;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: credentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}