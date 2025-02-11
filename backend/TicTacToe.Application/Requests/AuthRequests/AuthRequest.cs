namespace TicTacToe.Application.Requests.AuthRequests;

public class AuthRequest
{
    public AuthRequest()
    {
        
    }

    public AuthRequest(AuthRequest request)
    {
        Name = request.Name;
        Password = request.Password;
    }

    public string Name { get; set; } = default!;

    public string Password { get; set; } = default!;
}