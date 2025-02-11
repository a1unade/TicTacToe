namespace TicTacToe.Application.Requests;

public abstract class BaseRequest
{
    public bool RequiresAuthorization { get; protected set; } = true; 
}