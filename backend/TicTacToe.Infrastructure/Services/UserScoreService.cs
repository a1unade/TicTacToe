using Microsoft.Extensions.Options;
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
    
    public async Task<UserScore?> GetAsync(string id, CancellationToken cancellationToken)
    {
        return await _mongo.Find(user => user.Id.ToString() == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(UserScore newUserScore, CancellationToken cancellationToken)
    {
        await _mongo.InsertOneAsync(newUserScore, cancellationToken: cancellationToken);
    }

    public async Task UpdateUserScoreAsync(UserScore userScore)
    {
        var filter = Builders<UserScore>.Filter.Eq(u => u.UserIdPostgres, userScore.UserIdPostgres);
        var update = Builders<UserScore>.Update.Set(u => u.Score, userScore.Score);

        var result = await _mongo.UpdateOneAsync(filter, update);

        if (result.MatchedCount == 0)
        {
            throw new Exception($"User with ID {userScore.UserIdPostgres} not found.");
        }
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