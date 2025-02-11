namespace TicTacToe.Application.Responses;

public class BaseResponse
{
    public bool IsSuccessfully { get; set; } 
    
    public string? Message { get; set; }
    
    public Guid? EntityId { get; set; }
}