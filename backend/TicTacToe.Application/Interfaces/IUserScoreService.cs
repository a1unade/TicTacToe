using MongoDB.Bson;
using TicTacToe.Domain.MongoEntity;

namespace TicTacToe.Application.Interfaces;

public interface IUserScoreService
{
    Task<UserScore?> GetAsync(string id, CancellationToken cancellationToken);
    Task CreateAsync(UserScore newUserScore, CancellationToken cancellationToken);
    Task UpdateScoreAsync(Guid userIdPostgres, int newScore, CancellationToken cancellationToken);

    Task<UserScore?> GetByUserIdPostgresAsync(Guid userIdPostgres, CancellationToken cancellationToken);

    Task<List<UserScore>> GetTop10UsersAsync(CancellationToken cancellationToken);
}