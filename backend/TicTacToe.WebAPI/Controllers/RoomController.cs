using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Features.Rooms.GetRooms;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests.Games;

namespace TicTacToe.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDbContext _context;

    public RoomController(IMediator mediator, IDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    

    [HttpPost("getRooms")]
    public async Task<IActionResult> GetRooms(GetGamesPaginationRequest request, CancellationToken cancellationToken)
    {
        var rooms = await _mediator.Send(new GetGamesQuery(request), cancellationToken);
        return Ok(rooms);
    }

    [HttpPost("fafawf")]
    public async Task<IActionResult> Gets(Guid roomId, CancellationToken cancellationToken)
    {
        var t = await _context.Rooms
            .Include(x => x.Player1)
            .Include(x => x.Player2)
            .Include(r => r.Match) // Подгружаем связанный матч
            .FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

        return Ok(t);

    }
}