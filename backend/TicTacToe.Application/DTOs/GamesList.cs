namespace TicTacToe.Application.DTOs;

public class GamesList
{
    public List<GameDto> GameDtos { get; set; } = default!;
    
    public int TotalCount { get; set; }
}