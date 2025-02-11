using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Application.Features.Users.GetTopUsers;

public class GetUsersQueryHandler : IHandler<GetUsersQuery, TopUsers>
{
    private readonly IDbContext _context;
    private readonly IUserScoreService _scoreService;


    public GetUsersQueryHandler(IDbContext context, IUserScoreService scoreService)
    {
        _context = context;
        _scoreService = scoreService;
    }
    
    public async Task<TopUsers> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var topUsers = await _scoreService.GetTop10UsersAsync(cancellationToken);

        return new TopUsers { UsersDtosScores = topUsers };
    }
}