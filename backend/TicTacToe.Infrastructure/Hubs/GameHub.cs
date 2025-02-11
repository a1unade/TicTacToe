using Microsoft.AspNetCore.SignalR;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Infrastructure.Hubs;

public class GameHub : Hub
{
    private readonly IRoomService _roomService;

    public GameHub(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task<string> CreateOrJoinRoom(ConnectionDto connectionDto)
    {
        if (connectionDto.RoomId == null)
        {
            var roomId = await _roomService.CreateRoom(connectionDto);
            if (roomId == null)
            {
                return "User not found";
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString()!);

            await Clients.Group(roomId.ToString()!).SendAsync("PlayerJoined", new
            {
                PlayerId = connectionDto.Player1,
                Role = "Player1",
                RoomId = roomId,
                Message = "$Room created and connected with ID: {roomId}"
            });

            return roomId.ToString()!;
        }

        return connectionDto.RoomId.ToString()!;
    }

    public async Task<string> JoinRoom(JoinDto joinDto)
    {
        try
        {
            var room = await _roomService.GetRoomByIdAsync(joinDto.RoomId);

            if (room == null)
            {
                await Clients.Caller.SendAsync("Info", new
                {
                    Message = "Room not found",
                    RoomId = joinDto.RoomId
                });

                return "Room not found";
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, joinDto.RoomId.ToString());

            await Clients.Group(room.Id.ToString()).SendAsync("Info", new
            {
                Room = room,
                Creator = room.Player1,
                Role = "Player1",
                RoomId = joinDto.RoomId,
                Match = room.Match,
                Message = "Player joined the room"
            });

            return $"Connected to room {joinDto.RoomId}";
        }
        catch (Exception e)
        {
            await Clients.Group(joinDto.RoomId.ToString()).SendAsync("Info", new
            {
                RoomId = joinDto.RoomId,
                Message = "Player joined the room"
            });
            Console.WriteLine(e);
            return "Pizda";
        }
    }

    public async Task<string> JoinGame(JoinGameDto joinGame)
    {
        var room = await _roomService.GetRoomByIdAsync(joinGame.RoomId);

        if (room == null)
        {
            await Clients.Caller.SendAsync("Info", new
            {
                Message = "Room not found",
                RoomId = joinGame.RoomId
            });

            return "Room not found";
        }

        if (room.Player2Id == null)
        {
            var (message, success) = await _roomService.AddPlayerToRoomAsync(room.Id, joinGame.Player);
            if (success)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, joinGame.RoomId.ToString());
                await Clients.Group(room.Id.ToString()).SendAsync("PlayerJoined", new
                {
                    PlayerId = joinGame.Player,
                    Role = "Player2",
                    RoomId = room.Id
                });

                return $"Connected to room as Player 2 with ID: {joinGame.RoomId}";
            }

            return message;
        }

        return $"Место занято этим челом {room.Player2.Name}";
    }

    public async Task NotifyMove(Guid roomId, int position)
    {
        // Отправляем обновление о ходе всем участникам комнаты
        await Clients.Group(roomId.ToString()).SendAsync("MoveMade", position);
    }

    public async Task NotifyGameStatus(Guid roomId, string status)
    {
        // Отправляем обновление статуса игры всем участникам комнаты
        await Clients.Group(roomId.ToString()).SendAsync("GameStatusUpdated", status);
    }

    public async Task LeaveGame(Guid roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public async Task SendMove(Guid roomId, int position, Guid playerId)
    {
        // Логика обработки хода
        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMove", position, playerId);
    }
}