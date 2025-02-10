using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Application.Common;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Services;

namespace TicTacToe.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCqrs(this IServiceCollection services)
    {
        services.AddHandlers();
        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IAuthorizationService, AuthorizationService>();
        services.AddHttpContextAccessor();

    }
    
    private static void AddHandlers(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var handlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>)))
            .ToList();

        if (!handlerTypes.Any())
            throw new Exception();
        
        foreach (var types in handlerTypes)
        {
            var handlerInterfaces = types.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>));
            
            services.AddTransient(handlerInterfaces, types);
        }
    }
}