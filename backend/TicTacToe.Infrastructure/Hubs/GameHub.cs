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
            // Создаем новую комнату, если RoomId не указан
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
                RoomId = roomId
            });

            return $"Room created and connected with ID: {roomId}";
        }


        // Подключаемся к существующей комнате
        var room = await _roomService.GetRoomByIdAsync((Guid)connectionDto.RoomId);
        if (room == null)
        {
            await Clients.Group(connectionDto.RoomId.ToString()!).SendAsync("Info", new
            {
                Message = "Room not found",
                PlayerId = connectionDto.Player1,
                Role = "Player1",
                RoomId = connectionDto.RoomId,
                Math = room.Match
            });
            return "Room not found";
        }
        await Clients.Group(connectionDto.RoomId.ToString()!).SendAsync("Info", new
        {
            PlayerId = connectionDto.Player1,
            Role = "Player1",
            RoomId = connectionDto.RoomId,
            Math = room.Match,
            Player2 = room.Player2,
            Message = "Мы нашли комнату и пытаемя закинуть чела"
        });


        // // Проверяем, есть ли место для второго игрока
        // if (room.Player2Id == null)
        // {
        //     // Добавляем второго игрока, сразу подключаем его
        //     var (message, success) = await _roomService.AddPlayerToRoomAsync(room.Id, connectionDto.Player1);
        //     if (success)
        //     {
        //         await Groups.AddToGroupAsync(Context.ConnectionId, connectionDto.RoomId.ToString()!);
        //         await Clients.Group(room.Id.ToString()).SendAsync("PlayerJoined", new
        //         {
        //             PlayerId = connectionDto.Player1,
        //             Role = "Player2",
        //             RoomId = room.Id
        //         });
        //
        //         return $"Connected to room as Player 2 with ID: {connectionDto.RoomId}";
        //     }
        //     else
        //     {
        //         return message;
        //     }
        // }

        // Если комната заполнена, подключаемся как зритель
        await Groups.AddToGroupAsync(Context.ConnectionId, connectionDto.RoomId.ToString()!);
        await Clients.Group(room.Id.ToString()).SendAsync("INfo", new
        {
            PlayerId = connectionDto.Player1,
            Role = "Player2",
            RoomId = room.Id
        });
        return $"Connected to room as Spectator with ID: {connectionDto.RoomId}";
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