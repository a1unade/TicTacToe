namespace TicTacToe.Application.Requests.AuthRequests;

public class LoginRequest
{
    public LoginRequest()
    {
        
    }

    public LoginRequest(LoginRequest request)
    {
        Name = request.Name;
        Password = request.Password;
    }
    public string Name { get; set; } = default!;

    public string Password { get; set; } = default!;
}