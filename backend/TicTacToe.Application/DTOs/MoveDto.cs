namespace TicTacToe.Application.DTOs;

public class MoveDto
{
    public Guid RoomId { get; set; }
    public int Position { get; set; }
    public Guid PlayerId { get; set; }
    
}