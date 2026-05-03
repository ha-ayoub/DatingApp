using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Enums;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Users.Commands;

public record UpdatePreferencesCommand(
    List<Gender> GenderPreferences,
    int MinAge,
    int MaxAge,
    int MaxDistanceKm
) : IRequest;

public class UpdatePreferencesCommandHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser) : IRequestHandler<UpdatePreferencesCommand>
{
    public async Task Handle(UpdatePreferencesCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException("Not authenticated");
        var user = await uow.Users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("User not found", userId);

        user.UpdateProfile(
            user.Bio, user.City, user.Country,
            request.MinAge, request.MaxAge,
            request.MaxDistanceKm, request.GenderPreferences);

        uow.Users.Update(user);
        await uow.SaveChangesAsync(ct);
    }
}