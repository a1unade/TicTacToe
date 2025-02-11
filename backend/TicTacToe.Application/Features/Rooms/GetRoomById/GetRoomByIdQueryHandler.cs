using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Application.Features.Rooms.GetRoomById;

public class GetRoomByIdQueryHandler : IHandler<GetRoomByIdQuery, RoomForUi>
{
    private readonly IRoomService _service;
    private readonly IUserScoreService _userScoreService;

    public GetRoomByIdQueryHandler(IRoomService service, IUserScoreService userScoreService)
    {
        _service = service;
        _userScoreService = userScoreService;
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

        var firstUserScore = await _userScoreService.GetByUserIdPostgresAsync(room.Player1.Id, cancellationToken);
        var secondUserScore = await _userScoreService.GetByUserIdPostgresAsync(room.Player2.Id, cancellationToken);

        return new RoomForUi
        {
            IsSuccessfully = true,
            Message = "Комната найдена",
            FirstPlayer = new UsersDto
            {
                Score = firstUserScore?.Score ?? 0,
                Name = room.Player1?.Name ?? "Неизвестный",
                UserId = room.Player1?.Id ?? Guid.Empty,
                Symbol = 'X'
            },
            SecondPlayer = room.Player2 != null ? new UsersDto
            {
                Score = secondUserScore?.Score ?? 0,
                Name = room.Player2.Name,
                UserId = room.Player2.Id,
                Symbol = 'O'
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