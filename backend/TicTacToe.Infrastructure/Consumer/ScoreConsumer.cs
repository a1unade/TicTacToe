using MassTransit;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;

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
        await _scoreService.UpdateScoreAsync(context.Message.UserId, context.Message.NewScore, default);
    }
}