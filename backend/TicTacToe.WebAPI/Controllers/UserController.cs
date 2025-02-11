using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.Features.Users.GetTopUsers;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("topUsers")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUsersQuery(), cancellationToken);

        return Ok(response);
    }
}