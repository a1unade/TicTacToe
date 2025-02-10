namespace TicTacToe.Application.Interfaces;

public interface IAuthorizationService
{
    public (bool isValid, string message) Authorization();
}