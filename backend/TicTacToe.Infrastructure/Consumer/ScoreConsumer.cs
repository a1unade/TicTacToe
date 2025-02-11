using MassTransit;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.MongoEntity;

namespace TicTacToe.Infrastructure.Consumer;

public class ScoreConsumer : IConsumer<UpdateScoreDto>
{
    private readonly IUserScoreService _scoreService;

    public ScoreConsumer(IUserScoreService scoreService)
    {
        _scoreService = scoreService;
    }
    
    public async Task Consume(ConsumeContext<UpdateScoreDto> context)
    {
        var message = context.Message;

        var winnerScore = await _scoreService.GetByUserIdPostgresAsync(message.WinnerId, context.CancellationToken);
        if (winnerScore != null)
        {
            winnerScore.Score += 3; 
            await _scoreService.UpdateUserScoreAsync(new UserScore
            {
                Name = winnerScore.Name,
                Score = winnerScore.Score,
                UserIdPostgres = winnerScore.UserIdPostgres
            });
        }

        var loserScore = await _scoreService.GetByUserIdPostgresAsync(message.LoserId, context.CancellationToken);
        if (loserScore != null)
        {
            loserScore.Score -= 1;
            if (loserScore.Score < 0)
            {
                loserScore.Score = 0; 
            }
            await _scoreService.UpdateUserScoreAsync(new UserScore
            {
                Name = loserScore.Name,
                Score = loserScore.Score,
                UserIdPostgres = loserScore.UserIdPostgres
            });
        }
    }
}