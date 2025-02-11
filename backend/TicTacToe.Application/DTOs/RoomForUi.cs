using TicTacToe.Application.Responses;

namespace TicTacToe.Application.DTOs;

public class RoomForUi : BaseResponse
{
    public UsersDto FirstPlayer { get; set; } = default!;

    public UsersDto SecondPlayer { get; set; } = default!;

    public MatchDto Match { get; set; } = default!;
}