using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Application.Features.Rooms.GetRoomById;

public class GetRoomByIdQueryHandler : IHandler<GetRoomByIdQuery, RoomForUi>
{
    private readonly IRoomService _service;

    public GetRoomByIdQueryHandler(IRoomService service)
    {
        _service = service;
    }
    
    public async Task<RoomForUi> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _service.GetRoomByIdAsync(request.Id);

        if (room == null)
        {
            return new RoomForUi
            {
                IsSuccessfully = false,
                Message = "Не найдено"
            };
        }

        return new RoomForUi
        {
            IsSuccessfully = true,
            Message = "Комната найдена",
            FirstPlayer = new UsersDto
            {
                Score = room.Player1?.Score ?? 0,
                Name = room.Player1?.Name ?? "Неизвестный",
                UserId = room.Player1?.Id ?? Guid.Empty
            },
            SecondPlayer = room.Player2 != null ? new UsersDto
            {
                Score = room.Player2.Score,
                Name = room.Player2.Name,
                UserId = room.Player2.Id
            } : null!, 
            Match = room.Match != null ? new MatchDto
            {
                Board = room.Match.Board,
                Status = room.Match.Status,
                WinnerId = room.Match.WinnerId,
                CurrentPlayerId = room.Match.CurrentPlayerId,
                Score = room.Match.MaxScore
            } : null!
        };
    }

}