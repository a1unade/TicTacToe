namespace TicTacToe.Application.Requests.Games;

public class GetGamesPaginationRequest
{

    public GetGamesPaginationRequest()
    {
        
    }
    
    public GetGamesPaginationRequest(GetGamesPaginationRequest request)
    {
        Page = request.Page;
        Size = request.Size;
    }
    
    public int Page { get; set; }

    public int Size { get; set; }
}