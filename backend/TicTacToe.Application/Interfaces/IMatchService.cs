namespace TicTacToe.Application.Interfaces;

public interface IMatchService
{
    public Task<(string Board, string Status, Guid? NextPlayer, string Message)> ProcessMove(Guid roomId, Guid playerId,
        int position);

}