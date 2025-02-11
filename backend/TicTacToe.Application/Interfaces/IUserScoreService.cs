using TicTacToe.Domain.MongoEntity;

namespace TicTacToe.Application.Interfaces;

public interface IUserScoreService
{
    Task<UserScore?> GetAsync(string id, CancellationToken cancellationToken);
    Task CreateAsync(UserScore newUserScore, CancellationToken cancellationToken);
    Task UpdateUserScoreAsync(UserScore userScore);

    Task<UserScore?> GetByUserIdPostgresAsync(Guid userIdPostgres, CancellationToken cancellationToken);

    Task<List<UserScore>> GetTop10UsersAsync(CancellationToken cancellationToken);
}