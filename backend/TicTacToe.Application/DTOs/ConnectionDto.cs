namespace TicTacToe.Application.DTOs;

public class ConnectionDto
{
    public Guid Player1 { get; set; }
    
    public int? MaxScore { get; set; }
    
    public Guid? RoomId { get; set; }
    
    public int Player2Score { get; set; }
}