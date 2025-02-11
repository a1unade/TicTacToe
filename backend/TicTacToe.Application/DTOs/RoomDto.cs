namespace TicTacToe.Application.DTOs;

public class RoomDto
{
    public Guid Id { get; set; }
    public Guid Player1Id { get; set; }
    public Guid? Player2Id { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Board { get; set; }
}