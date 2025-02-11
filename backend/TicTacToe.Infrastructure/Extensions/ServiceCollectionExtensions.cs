using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Application.Interfaces;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>()
            .AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddSignalR();
    }
}