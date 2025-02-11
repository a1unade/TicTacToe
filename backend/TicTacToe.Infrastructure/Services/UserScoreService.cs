using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.MongoEntity;
using TicTacToe.Infrastructure.Options;

namespace TicTacToe.Infrastructure.Services;

public class UserScoreService : IUserScoreService
{
    private readonly IMongoCollection<UserScore> _mongo;

    public UserScoreService(IOptions<MongoOptions> mongoDatabaseSettings)
    {
        var mongoClient = new MongoClient(mongoDatabaseSettings.Value.ConnectionString);
        var mongoData = mongoClient.GetDatabase(mongoDatabaseSettings.Value.DatabaseName);
        _mongo = mongoData.GetCollection<UserScore>(mongoDatabaseSettings.Value.CollectionName);
    }

    public async Task<List<UserScore>> GetAsync(CancellationToken cancellationToken)
    {
        return await _mongo.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<UserScore?> GetAsync(string id, CancellationToken cancellationToken)
    {
        return await _mongo.Find(user => user.Id.ToString() == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(UserScore newUserScore, CancellationToken cancellationToken)
    {
        await _mongo.InsertOneAsync(newUserScore, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(ObjectId id, UserScore updatedUserScore, CancellationToken cancellationToken)
    {
        await _mongo.ReplaceOneAsync(user => user.Id == id, updatedUserScore, cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        await _mongo.DeleteOneAsync(user => user.Id.ToString() == id, cancellationToken);
    }
    
    public async Task<UserScore?> GetByUserIdPostgresAsync(Guid userIdPostgres, CancellationToken cancellationToken)
    {
        return await _mongo.Find(user => user.UserIdPostgres == userIdPostgres).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<List<UserScore>> GetTop10UsersAsync(CancellationToken cancellationToken)
    {
        return await _mongo.Find(_ => true)
            .SortByDescending(user => user.Score)
            .Limit(10)
            .ToListAsync(cancellationToken);
    }
}