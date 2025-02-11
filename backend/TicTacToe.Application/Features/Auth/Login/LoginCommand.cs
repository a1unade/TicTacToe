using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests;
using TicTacToe.Application.Requests.AuthRequests;
using TicTacToe.Application.Responses;

namespace TicTacToe.Application.Features.Auth.Login;

public class LoginCommand : LoginRequest, IRequest<AuthResponse>
{
    public LoginCommand(LoginRequest request) : base(request)
    {
        
    }
}