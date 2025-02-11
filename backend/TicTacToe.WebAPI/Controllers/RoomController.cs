using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.Features.Rooms.GetRooms;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests.Games;

namespace TicTacToe.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    [HttpPost("getRooms")]
    public async Task<IActionResult> GetRooms(GetGamesPaginationRequest request, CancellationToken cancellationToken)
    {
        var rooms = await _mediator.Send(new GetGamesQuery(request), cancellationToken);
        return Ok(rooms);
    }
    
}