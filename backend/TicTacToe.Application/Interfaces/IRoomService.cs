using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Interfaces;

public interface IRoomService
{
    public Task<Guid?> CreateRoom(ConnectionDto dto);

    public Task<Room?> GetRoomByIdAsync(Guid roomId);

    public Task<(string, bool)> AddPlayerToRoomAsync(Guid roomId, Guid playerId);
}