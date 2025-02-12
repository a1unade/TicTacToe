using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Application.Interfaces;
using TicTacToe.Infrastructure.Consumer;
using TicTacToe.Infrastructure.Options;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtService, JwtService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IRoomService, RoomService>()
            .AddScoped<IMatchService, MatchService>()
            .AddSingleton<IUserScoreService, UserScoreService>();

        services.AddSignalR();
        
        services.AddMessageBus(configuration);
        
        services.AddMongo(configuration);
    }
    
    private static void AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(nameof(RabbitOptions)).Get<RabbitOptions>()!;
        
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<ScoreConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host($"rabbitmq://{options.Hostname}", h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
    }
    
    private static void AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoOptions>(configuration.GetSection("MongoOptions"));
    }
}