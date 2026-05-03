using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Users.Commands;

public record UpdateProfileCommand(
    string? Bio,
    string? City,
    string? Country
) : IRequest;

public class UpdateProfileCommandHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser) : IRequestHandler<UpdateProfileCommand>
{
    public async Task Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException("Not authenticated");
        var user = await uow.Users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("User not found", userId);

        user.UpdateProfile(
            request.Bio, request.City, request.Country,
            user.MinAgePreference, user.MaxAgePreference,
            user.MaxDistanceKm, user.GenderPreferences);

        uow.Users.Update(user);
        await uow.SaveChangesAsync(ct);
    }
}