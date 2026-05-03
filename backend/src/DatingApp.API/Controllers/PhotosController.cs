using DatingApp.Application.Features.Photos.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PhotosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> AddPhoto(IFormFile file, CancellationToken ct)
        => Ok(await mediator.Send(new AddPhotoCommand(file), ct));

    [HttpDelete("{photoId:guid}")]
    public async Task<IActionResult> DeletePhoto(Guid photoId, CancellationToken ct)
    {
        await mediator.Send(new DeletePhotoCommand(photoId), ct);
        return NoContent();
    }

    [HttpPut("{photoId:guid}/main")]
    public async Task<IActionResult> SetMain(Guid photoId, CancellationToken ct)
    {
        await mediator.Send(new SetMainPhotoCommand(photoId), ct);
        return NoContent();
    }
}