namespace TicTacToe.Application.DTOs;

public class GameDto
{
    public Guid Id { get; set; }

    public string CreatorUserName { get; set; } = default!;
    
    public Guid CreatorId { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = default!;
    
    public bool CanJoin { get; set; }
    
    public int? MaxScore { get; set; }
}