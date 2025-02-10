namespace TicTacToe.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    
    public Guid Player1Id { get; set; } 
    
    public Guid Player2Id { get; set; }

    public string Status { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
}