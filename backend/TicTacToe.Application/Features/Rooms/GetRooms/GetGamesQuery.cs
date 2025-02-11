using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests.Games;

namespace TicTacToe.Application.Features.Rooms.GetRooms;

public class GetGamesQuery : GetGamesPaginationRequest, IRequest<GamesList>
{
    public GetGamesQuery(GetGamesPaginationRequest request) : base(request)
    {
        
    }
}