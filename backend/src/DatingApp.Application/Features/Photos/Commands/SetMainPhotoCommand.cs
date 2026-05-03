using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Photos.Commands;

public record SetMainPhotoCommand(Guid PhotoId) : IRequest;

public class SetMainPhotoCommandHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser) : IRequestHandler<SetMainPhotoCommand>
{
    public async Task Handle(SetMainPhotoCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException("Not authenticated");
        var user = await uow.Users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("User not found", userId);

        var target = user.Photos.FirstOrDefault(p => p.Id == request.PhotoId)
            ?? throw new NotFoundException("Photo not found", userId);

        foreach (var p in user.Photos) p.IsMain = false;
        target.IsMain = true;

        uow.Users.Update(user);
        await uow.SaveChangesAsync(ct);
    }
}