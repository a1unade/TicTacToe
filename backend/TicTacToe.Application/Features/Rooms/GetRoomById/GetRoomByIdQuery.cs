using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests;

namespace TicTacToe.Application.Features.Rooms.GetRoomById;

public class GetRoomByIdQuery : IdRequest, IRequest<RoomForUi>
{
    public GetRoomByIdQuery(IdRequest request) : base(request)
    {
        
    }
}