using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Services;
public class RoomService : IRoomService
{
    private readonly IDbContext _context;
    private readonly IUserScoreService _userScoreService;

    public RoomService(IDbContext context, IUserScoreService userScoreService)
    {
        _context = context;
        _userScoreService = userScoreService;
    }

    public async Task<Guid?> CreateRoom(ConnectionDto connectionDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == connectionDto.Player1);

        if (user == null)
        {
            return null;
        }

        var room = new Room
        {
            Player1Id = connectionDto.Player1,
            Status = "Waiting",
            CreatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        var match = new Match
        {
            IsStarted = false,
            CreatedAt = DateTime.UtcNow,
            Status = "Waiting",
            MaxScore = connectionDto.MaxScore ?? 0,
            RoomId = room.Id,
            Board = "---------",
            CurrentPlayerId = connectionDto.Player1
        };

        _context.Matches.Add(match);
        await _context.SaveChangesAsync();

        return room.Id;
    }

    public async Task<Room?> GetRoomByIdAsync(Guid roomId)
    {
        return await _context.Rooms
            .AsNoTracking()
            .Include(x => x.Player1)
            .Include(x => x.Player2)
            .Include(r => r.Match) // Подгружаем связанный матч
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    public async Task<(string, bool)> AddPlayerToRoomAsync(Guid roomId, Guid playerId)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        var player2 =  _context.Users.FirstOrDefault(x => x.Id == playerId);
        var match = await _context.Matches.FirstOrDefaultAsync(m => m.RoomId == roomId);
        
        if (player2 == null)
        {
            return ("Пользователь не найден", false);
        }
        
        var userScore = await _userScoreService.GetByUserIdPostgresAsync(player2.Id, default);
        
        if (room == null || room.Player2Id != null)
        {
            return ("комната налл или уже заполнена", false); 
        }

        // Добавляем второго игрока
        room.Player2Id = playerId;
        room.Status = "InGame"; 

        
        if (match != null)
        {
            if (match.MaxScore < userScore?.Score)
            {
                return ("У игрока рейтинг больше чем у комнаты", false);
            }
            
            match.IsStarted = true;
            match.Status = "InProgress";
        }

        await _context.SaveChangesAsync();
        return ("Нормально живем", true);
    }
}