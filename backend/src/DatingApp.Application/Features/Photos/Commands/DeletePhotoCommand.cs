using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Photos.Commands;

public record DeletePhotoCommand(Guid PhotoId) : IRequest;

public class DeletePhotoCommandHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser,
    ICloudinaryService cloudinary) : IRequestHandler<DeletePhotoCommand>
{
    public async Task Handle(DeletePhotoCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException("Not authenticated");

        var photo = await uow.Photos.GetByIdAsync(request.PhotoId, ct)
            ?? throw new NotFoundException("Photo not found", userId);

        if (photo.UserId != userId)
            throw new UnauthorizedException("Not your photo");

        if (photo.IsMain)
            throw new ArgumentException("Cannot delete your main photo");

        await cloudinary.DeletePhotoAsync(photo.PublicId, ct);
        uow.Photos.Remove(photo);
        await uow.SaveChangesAsync(ct);
    }
}