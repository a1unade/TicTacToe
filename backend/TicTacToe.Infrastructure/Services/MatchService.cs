using MassTransit;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Services;

public class MatchService : IMatchService
{
    private readonly IDbContext _context;
    private readonly IBus _bus;

    public MatchService(IDbContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<(string Board, string Status, Guid? NextPlayer, string Message)> ProcessMove(MoveDto moveDto)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(m => m.RoomId == moveDto.RoomId);
        if (match == null) return ("", "Error", null, "Матч не найден");

        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == moveDto.RoomId);
        if (room == null) return ("", "Error", null, "Комната не найдена");

        // Определяем, кто ходит первым
        if (match.CurrentPlayerId == Guid.Empty)
        {
            match.CurrentPlayerId = room.Player1Id;
        }

        // Проверяем, чей сейчас ход
        if (match.CurrentPlayerId != moveDto.PlayerId)
            return (match.Board, match.Status, match.CurrentPlayerId, "Сейчас не ваш ход!");

        // Проверяем, свободна ли ячейка
        if (match.Board[moveDto.Position] != '-')
            return (match.Board, match.Status, match.CurrentPlayerId, "Эта клетка уже занята!");

        // Обновляем доску
        var boardArray = match.Board.ToCharArray();
        boardArray[moveDto.Position] = (moveDto.PlayerId == room.Player1Id) ? 'X' : 'O';
        match.Board = new string(boardArray);

        // Проверяем, есть ли победитель
        string? winner = CheckWinner(match.Board);
        if (winner != null)
        {
            match.Status = "GameOver";
            match.WinnerId = moveDto.PlayerId;
            await _context.SaveChangesAsync();
            Guid loserId = match.CurrentPlayerId == room.Player1Id ? room.Player2Id ?? Guid.Empty : room.Player1Id;
            await UpdateScores(moveDto.PlayerId, loserId);
            return (match.Board, "GameOver", null, $"Победитель: {winner}");
        }

        // Проверяем на ничью
        if (!match.Board.Contains('-'))
        {
            match.Status = "Draw";
            await _context.SaveChangesAsync();
            return (match.Board, "Draw", null, "Ничья!");
        }

        // Смена игрока
        match.CurrentPlayerId = (match.CurrentPlayerId == room.Player1Id) ? room.Player2Id ?? room.Player1Id : room.Player1Id;

        await _context.SaveChangesAsync();

        return (match.Board, "InProgress", match.CurrentPlayerId, "Ход принят!");
    }

    private async Task UpdateScores(Guid winnerId, Guid loserId)
    {
        await _bus.Publish(new UpdateScoreDto { WinnerId = winnerId, LoserId = loserId });
    }
    
    private static string? CheckWinner(string board)
    {
        int[][] winPatterns =
        {
            new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 }, 
            new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 }, 
            new[] { 0, 4, 8 }, new[] { 2, 4, 6 } 
        };

        foreach (var pattern in winPatterns)
        {
            if (board[pattern[0]] != '-' && board[pattern[0]] == board[pattern[1]] &&
                board[pattern[1]] == board[pattern[2]])
            {
                return board[pattern[0]].ToString();
            }
        }

        return null;
    }
    
    public async Task<(string Board, string Status, Guid PlayerId, char Symbol)> StartNewRound(Guid roomId)
    {
        var room = await _context.Rooms
            .Include(r => r.Player1)
            .Include(r => r.Player2)
            .Include(x => x.Match)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null) throw new ArgumentException();
        if (room.Match == null) throw new ArgumentException();
        
        var newMatch = new Match
        {
            RoomId = room.Id,
            Board = "---------", 
            Status = "InProgress",
            CurrentPlayerId = room.Player1Id, 
            MaxScore = room.Match.MaxScore 
        };

        _context.Matches.Add(newMatch);
        await _context.SaveChangesAsync();

        room.Match = newMatch;

        await _context.SaveChangesAsync();

        return (newMatch.Board, newMatch.Status, room.Player1.Id, 'X');
    }
}
