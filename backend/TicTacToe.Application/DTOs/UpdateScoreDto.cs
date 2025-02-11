namespace TicTacToe.Application.DTOs;

public class UpdateScoreDto
{
    public Guid WinnerId { get; set; }
    
    public Guid LoserId { get; set; }
}