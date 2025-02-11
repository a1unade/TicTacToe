using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Application.Features.Rooms.GetRooms;

public class GetGamesQueryHandler : IHandler<GetGamesQuery, GamesList>
{
    private readonly IDbContext _context;

    public GetGamesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<GamesList> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Rooms
            .Include(r => r.Player1)
            .Include(r => r.Match)
            .OrderByDescending(r => r.Status == "Waiting" || r.Status == "InGame" ? 1 : 0) 
            .ThenByDescending(r => r.CreatedAt); 

        var totalCount = await query.CountAsync(cancellationToken);

        var games = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(r => new GameDto
            {
                Id = r.Id,
                CreatorUserName = r.Player1.Name,
                CreatorId = r.Player1.Id,
                CreatedAt = r.CreatedAt,
                Status = r.Status,
                CanJoin = r.Status == "Waiting" ,
                MaxScore = r.Match.MaxScore,
            })
            .ToListAsync(cancellationToken);

        return new GamesList
        {
            GameDtos = games,
            TotalCount = totalCount
        };
    }
}