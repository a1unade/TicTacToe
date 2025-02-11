using TicTacToe.Application.DTOs;

namespace TicTacToe.Application.Interfaces;

public interface IMatchService
{
    public Task<(string Board, string Status, Guid? NextPlayer, string Message)> ProcessMove(MoveDto moveDto);

    public Task<(string Board, string Status, Guid PlayerId, char Symbol)> StartNewRound(Guid roomId);
}