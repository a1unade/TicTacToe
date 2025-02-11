using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TicTacToe.Persistence.EfContext;

public class EfFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder().Build();
        
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionBuilder.UseNpgsql("Host=db;Port=5432;Username=postgres;Password=tictactoe;Database=Tic;");

        return new ApplicationDbContext(optionBuilder.Options);
    }
}