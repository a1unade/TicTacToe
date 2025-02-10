namespace TicTacToe.Application.Interfaces;

public interface IDbSeeder
{
    public Task SeedAsync(IDbContext context, CancellationToken cancellationToken = default);
}