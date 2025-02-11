namespace TicTacToe.Application.Requests.Room;

public class CreateRoomRequest
{
    public CreateRoomRequest()
    {
        
    }

    public CreateRoomRequest(CreateRoomRequest request)
    {
        Player1Id = request.Player1Id;
    }
    
    public Guid Player1Id { get; set; }
}