using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Interfaces;

public interface IJwtService
{
    public string GenerateToken(User user, int score);
}