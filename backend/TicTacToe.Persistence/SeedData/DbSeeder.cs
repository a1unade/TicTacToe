using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.SeedData;

public class DbSeeder : IDbSeeder
{
    private readonly IPasswordHasher _passwordHasher;

    public DbSeeder(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    public async Task SeedAsync(IDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Users.AnyAsync(cancellationToken))
        {
            return; 
        }

        var users = new List<User>
        {
            new()
            {
                Name = "Bool",
                PasswordHash = _passwordHasher.HashPassword("Bool"),
                Score = 100
            },
            new ()
            {
                Name = "M9s0",
                PasswordHash = _passwordHasher.HashPassword("M9s0"),
                Score = 200
            },
            new ()
            {
                Name = "Mesi",
                PasswordHash = _passwordHasher.HashPassword("Mesi"),
                Score = 300
            }
        };

        await context.Users.AddRangeAsync(users, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}