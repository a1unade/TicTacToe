using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Interfaces;
using TicTacToe.Persistence.EfContext;

namespace TicTacToe.Persistence.MigrationTools;

public class Migrator
{
    private readonly ApplicationDbContext _context;
    private readonly IDbSeeder _seeder;

    public Migrator(ApplicationDbContext context, IDbSeeder seeder)
    {
        _context = context;
        _seeder = seeder;
    }
    
    public async Task MigrateAsync()
    {
        try
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            await _seeder.SeedAsync(_context);
        }
        catch (Exception e)
        {
            Console.WriteLine($"migrations apply failed {e.Message}");
            throw;
        }
    }
}