using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests;
using TicTacToe.Application.Requests.AuthRequests;
using TicTacToe.Application.Responses;

namespace TicTacToe.Application.Features.Auth.Authorization;

public class AuthCommand : AuthRequest, IRequest<AuthResponse>
{
    public AuthCommand(AuthRequest request) : base(request)
    {
        
    }
}