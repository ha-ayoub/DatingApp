using DatingApp.Application.Features.Matches.Commands;
using DatingApp.Application.Features.Matches.Queries;
using DatingApp.Application.Features.Messages.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MatchesController(IMediator mediator) : ControllerBase
{
    [HttpPost("swipe")]
    public async Task<IActionResult> Swipe([FromBody] SwipeCommand cmd, CancellationToken ct)
        => Ok(await mediator.Send(cmd, ct));

    [HttpGet]
    public async Task<IActionResult> GetMatches([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetUserMatchesQuery(page, pageSize), ct));

    [HttpGet("{matchId:guid}/messages")]
    public async Task<IActionResult> GetMessages(Guid matchId, [FromQuery] int page = 1, [FromQuery] int pageSize = 30, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetMessagesQuery(matchId, page, pageSize), ct));
}
