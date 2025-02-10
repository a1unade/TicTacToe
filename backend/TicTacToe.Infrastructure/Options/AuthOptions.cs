using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TicTacToe.Infrastructure.Options;

public class AuthOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecretKey { get; set; } = default!;

    public SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new (Encoding.UTF8.GetBytes(SecretKey));
}