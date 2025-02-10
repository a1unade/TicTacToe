namespace TicTacToe.Application.Interfaces;

public interface IMediator
{
    Task<TResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellationToken);
}