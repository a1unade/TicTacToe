using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Application.Features.Users.GetTopUsers;

public class GetUsersQueryHandler : IHandler<GetUsersQuery, TopUsers>
{
    private readonly IDbContext _context;

    public GetUsersQueryHandler(IDbContext context)
    {
        _context = context;
    }
    
    public async Task<TopUsers> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var topUsers = await _context.Users
            .OrderByDescending(u => u.Score)
            .Take(10) 
            .Select(u => new UsersDto
            {
                UserId = u.Id,
                Name = u.Name,
                Score = u.Score
            })
            .ToListAsync(cancellationToken);

        return new TopUsers { UsersDtos = topUsers };
    }
}