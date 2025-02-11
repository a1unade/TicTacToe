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
        if (connectionDto.RoomId == null && connectionDto.MaxScore != null)
        {
            // Создаем новую комнату, если RoomId не указан
            var roomId = await _roomService.CreateRoom(connectionDto);
            if (roomId == null)
            {
                return "User not found";
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString()!);
            return $"Room created and connected with ID: {roomId}";
        }

        // Подключаемся к существующей комнате
        var room = await _roomService.GetRoomByIdAsync(connectionDto.RoomId!.Value);
        if (room == null)
        {
            return "Room not found";
        }

        // Проверяем, есть ли место для второго игрока
        if (room.Player2Id == null)
        {
            // Отправляем запрос на подтверждение первому игроку
            await Clients.Group(room.Id.ToString())
                .SendAsync("RequestToJoin", Context.ConnectionId, connectionDto.Player1);

            // Ждем подтверждения от первого игрока
            return "Waiting for confirmation from Player 1";
        }

        // Если комната заполнена, подключаемся как зритель
        await Groups.AddToGroupAsync(Context.ConnectionId, connectionDto.RoomId.ToString()!);
        return $"Connected to room as Spectator with ID: {connectionDto.RoomId}";
    }

    
    
    
    
    public async Task ConfirmJoinRoom(Guid roomId, Guid playerId, bool isConfirmed)
    {
        var room = await _roomService.GetRoomByIdAsync(roomId);
        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", "Room not found");
            return;
        }

        if (isConfirmed)
        {
            // Добавляем второго игрока
            var (message, success) = await _roomService.AddPlayerToRoomAsync(roomId, playerId);
            if (success)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                await Clients.Group(roomId.ToString()).SendAsync("PlayerJoined", playerId);
            }

            await Clients.Caller.SendAsync("Error", message);
        }

        // Отклоняем запрос, подключаем как зрителя
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString()!);
        await Clients.Caller.SendAsync("ConnectedAsSpectator");
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