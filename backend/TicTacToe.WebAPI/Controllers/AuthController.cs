using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.Features.Auth.Authorization;
using TicTacToe.Application.Features.Auth.Login;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Requests;
using TicTacToe.Application.Requests.AuthRequests;
using TicTacToe.Domain.Entities;

namespace TicTacToe.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDbContext _context;

    public AuthController(IMediator mediator, IDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new LoginCommand(request), cancellationToken);
        
        if (response.IsSuccessfully)
        {
            return Ok(response);
        }

        return BadRequest(response.Message);
    }
    
    [HttpPost("auth")]
    public async Task<IActionResult> AuthUser(AuthRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new AuthCommand(request), cancellationToken);
        
        if (response.IsSuccessfully)
        {
            return Ok(response);
        }

        return BadRequest(response.Message);
    }
    
    [HttpPost("yyy")]
    public async Task<IActionResult> ttt(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = new User(request.Name, request.Password);

        await _context.Users.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}