namespace TicTacToe.Application.Requests;

public class IdRequest
{
    public IdRequest()
    {
        
    }

    public IdRequest(IdRequest request)
    {
        Id = request.Id;
    }
    
    public Guid Id { get; set; }
}