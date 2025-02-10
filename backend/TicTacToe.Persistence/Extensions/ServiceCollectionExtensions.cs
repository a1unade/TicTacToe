using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Application.Interfaces;
using TicTacToe.Persistence.EfContext;
using TicTacToe.Persistence.MigrationTools;
using TicTacToe.Persistence.SeedData;

namespace TicTacToe.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
    }
    
    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IDbContext, ApplicationDbContext>()
            .AddScoped<IDbSeeder, DbSeeder>()
            .AddTransient<Migrator>();
    }
    
}