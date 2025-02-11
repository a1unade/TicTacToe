namespace TicTacToe.Application.DTOs;

public class JoinGameDto
{
    public Guid Player { get; set; }
    
    public Guid RoomId { get; set; }
}