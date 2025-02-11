using MongoDB.Bson;
using TicTacToe.Domain.MongoEntity;

namespace TicTacToe.Application.Interfaces;

public interface IUserScoreService
{
    Task<List<UserScore>> GetAsync(CancellationToken cancellationToken);
    Task<UserScore?> GetAsync(string id, CancellationToken cancellationToken);
    Task CreateAsync(UserScore newUserScore, CancellationToken cancellationToken);
    Task UpdateAsync(ObjectId id, UserScore updatedUserScore, CancellationToken cancellationToken);
    Task RemoveAsync(string id, CancellationToken cancellationToken);

    Task<UserScore?> GetByUserIdPostgresAsync(Guid userIdPostgres, CancellationToken cancellationToken);

    Task<List<UserScore>> GetTop10UsersAsync(CancellationToken cancellationToken);
}