using TicTacToe.Domain.MongoEntity;

namespace TicTacToe.Application.DTOs;

public class TopUsers
{
    public List<UserScore> UsersDtosScores { get; set; } = default!;
}