using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Entities;
using DatingApp.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Application.Features.Photos.Commands;

public record AddPhotoCommand(IFormFile File) : IRequest<PhotoResultDto>;
public record PhotoResultDto(Guid Id, string Url, bool IsMain);

public class AddPhotoCommandHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser,
    ICloudinaryService cloudinary) : IRequestHandler<AddPhotoCommand, PhotoResultDto>
{
    public async Task<PhotoResultDto> Handle(AddPhotoCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException("Not authenticated");
        var user = await uow.Users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("User not found", userId);

        if (request.File.Length > 5 * 1024 * 1024)
            throw new ArgumentException("File size must be less than 5MB");

        await using var stream = request.File.OpenReadStream();
        var (publicId, url) = await cloudinary.UploadPhotoAsync(stream, request.File.FileName, ct);

        var hasMainPhoto = user.Photos.Any(p => p.IsMain);

        var photo = new Photo
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Url = url,
            PublicId = publicId,
            IsMain = !hasMainPhoto,
            Order = user.Photos.Count + 1
        };

        await uow.Photos.AddAsync(photo, ct);
        await uow.SaveChangesAsync(ct);

        return new PhotoResultDto(photo.Id, photo.Url, photo.IsMain);
    }
}