using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Services;

public class MatchService : IMatchService
{
    private readonly IDbContext _context;

    public MatchService(IDbContext context)
    {
        _context = context;
    }

    public async Task<(string Board, string Status, Guid? NextPlayer, string Message)> ProcessMove(MoveDto moveDto)
    {
        var match = await _context.Matches
            .Include(x => x.CurrentPlayerId)
            .FirstOrDefaultAsync(m => m.RoomId == moveDto.RoomId);
        
        if (match == null) return ("", "Error", null, "Матч не найден");

        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == moveDto.RoomId);
        if (room == null) return ("", "Error", null, "Комната не найдена");

        // Проверяем, чей сейчас ход
        if (match.CurrentPlayerId != moveDto.PlayerId)
            return (match.Board, match.Status, match.CurrentPlayerId, "Сейчас не ваш ход!");

        // Проверяем, свободна ли ячейка
        if (match.Board[moveDto.Position] != '-')
            return (match.Board, match.Status, match.CurrentPlayerId, "Эта клетка уже занята!");

        // Обновляем доску
        var boardArray = match.Board.ToCharArray();
        boardArray[moveDto.Position] = moveDto.PlayerId == room.Player1Id ? 'X' : 'O';
        match.Board = new string(boardArray);

        // Проверяем, есть ли победитель
        string? winner = CheckWinner(match.Board);
        if (winner != null)
        {
            match.Status = "GameOver";
            await _context.SaveChangesAsync();
            //await UpdateScoresAndNotify(match.RoomId, winner);
            return (match.Board, match.Status, null, $"Победитель: {winner}");
        }

        // Проверяем на ничью
        if (!match.Board.Contains('-'))
        {
            match.Status = "Draw";
            await _context.SaveChangesAsync();
           // await NotifyDraw(match.RoomId);
            return (match.Board, match.Status, null, "Ничья!");
        }

        // Смена игрока
        match.CurrentPlayerId =
            (match.CurrentPlayerId == room.Player1Id) ? room.Player2Id ?? room.Player1Id : room.Player1Id;

        await _context.SaveChangesAsync();

        return (match.Board, "InProgress", match.CurrentPlayerId, "Ход принят!");
    }

    private async Task UpdateScoresAndNotify(Guid roomId, string winner)
    {
        
        
        // /// ТУт можно добавить монго или ребит 
        // var room = await _context.Rooms
        //     .Include(r => r.Player1)
        //     .Include(r => r.Player2)
        //     .FirstOrDefaultAsync(r => r.Id == roomId);
        //
        // if (room == null) return;
        //
        // var winnerPlayer = winner == "X" ? room.Player1 : room.Player2;
        // var loserPlayer = winner == "X" ? room.Player2 : room.Player1;
        //
        // if (winnerPlayer != null)
        // {
        //     winnerPlayer.Score += 3; 
        // }
        //
        // if (loserPlayer != null)
        // {
        //     loserPlayer.Score -= 1; 
        //     if (loserPlayer.Score < 0)
        //     {
        //         loserPlayer.Score = 0; 
        //     }
        // }
        //
        // await _context.SaveChangesAsync();
    }

    private async Task NotifyDraw(Guid roomId)
    {
        
        /// ТУт можно добавить монго или ребит 

        
        
        var room = await _context.Rooms
            .Include(r => r.Player1)
            .Include(r => r.Player2)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null) return;

        // Отправляем уведомление о ничьей
        // Вызываем метод в хабе через интерфейс для отправки уведомлений клиенту
    }

    private static string? CheckWinner(string board)
    {
        int[][] winPatterns =
        {
            new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 }, // Горизонтали
            new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 }, // Вертикали
            new[] { 0, 4, 8 }, new[] { 2, 4, 6 } // Диагонали
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

        // Сохраняем новый матч
        _context.Matches.Add(newMatch);
        await _context.SaveChangesAsync();

        // Обновляем комнату, чтобы она ссылается на новый матч
        room.Match = newMatch;

        await _context.SaveChangesAsync();

        return (newMatch.Board, newMatch.Status, room.Player1.Id, 'X');
    }
}
