using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

}