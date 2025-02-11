namespace TicTacToe.Application.DTOs;

public class UpdateScoreDto
{
    public int NewScore { get; set; }
    
    public Guid UserId { get; set; }
}