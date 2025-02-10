namespace TicTacToe.Application.Responses;

public class AuthResponse : BaseResponse
{
    public string Token { get; set; } = default!;
}