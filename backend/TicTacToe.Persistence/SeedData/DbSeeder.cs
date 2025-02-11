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
            },
            new ()
            {
                Name = "M9s0",
                PasswordHash = _passwordHasher.HashPassword("M9s0"),
            },
            new ()
            {
                Name = "Mesi",
                PasswordHash = _passwordHasher.HashPassword("Mesi"),
            }
        };

        await context.Users.AddRangeAsync(users, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}