namespace TicTacToe.Application.Requests.Room;

public class JoinRoomRequest
{

    public JoinRoomRequest()
    {
        
    }

    public JoinRoomRequest(JoinRoomRequest request)
    {
        RoomId = request.RoomId;
        Player2Id = request.Player2Id;
    }
    
    public Guid RoomId { get; set; }
    public Guid Player2Id { get; set; }
}