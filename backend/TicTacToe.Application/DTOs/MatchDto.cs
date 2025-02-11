namespace TicTacToe.Application.DTOs;

public class MatchDto
{
    public string Board { get; set; } = default!;

    public string Status { get; set; } = default!;
    
    public Guid? WinnerId { get; set; }
    
    public Guid CurrentPlayerId { get; set; }
    
    public int Score { get; set; }
}