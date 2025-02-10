using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests;
using TicTacToe.Application.Responses;

namespace TicTacToe.Application.Features.Authorization;

public class AuthCommand : AuthRequest, IRequest<AuthResponse>
{
    public AuthCommand(AuthRequest request) : base(request)
    {
        
    }
}