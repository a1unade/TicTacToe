namespace TicTacToe.Application.Interfaces;

public interface IHandler<TRequest, TResult>
{
    Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
}