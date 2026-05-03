using DatingApp.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand cmd, CancellationToken ct)
        => Ok(await mediator.Send(cmd, ct));

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand cmd, CancellationToken ct)
        => Ok(await mediator.Send(cmd, ct));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand cmd, CancellationToken ct)
        => Ok(await mediator.Send(cmd, ct));
}
