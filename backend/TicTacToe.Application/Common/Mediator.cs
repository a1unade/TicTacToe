using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests;

namespace TicTacToe.Application.Common;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAuthorizationService _authorization;

    public Mediator(IServiceProvider serviceProvider, IAuthorizationService authorization)
    {
        _serviceProvider = serviceProvider;
        _authorization = authorization;
    }

    public async Task<TResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellationToken)
    {
        if (request is BaseRequest baseRequest && baseRequest.RequiresAuthorization)
        {
            var (isValid, message) = _authorization.Authorization();
            if (!isValid)
            {
                throw new UnauthorizedAccessException(message);
            }
        }
        
        var requestType = request.GetType();
        var handlerType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResult));
        var handler = _serviceProvider.GetService(handlerType)
                      ?? throw new InvalidOperationException($"Handler: {requestType.Name} not found.");

        var handleMethod = handlerType.GetMethod("Handle");
        if (handleMethod == null)
            throw new InvalidOperationException($"Handler: {handlerType.Name} does not implement Handle method.");

        return await (Task<TResult>)handleMethod.Invoke(handler, new object[] { request, cancellationToken })!;
    }
}