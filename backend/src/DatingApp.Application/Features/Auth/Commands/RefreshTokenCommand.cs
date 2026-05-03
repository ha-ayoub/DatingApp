using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Auth.DTOs;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;

public class RefreshTokenCommandHandler(IUnitOfWork uow, IJwtService jwt) : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var user = await uow.Users.GetByRefreshTokenAsync(request.RefreshToken, ct)
            ?? throw new UnauthorizedException("Invalid refresh token.");

        if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedException("Refresh token expired.");

        var newRefreshToken = jwt.GenerateRefreshToken();
        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(30));
        uow.Users.Update(user);
        await uow.SaveChangesAsync(ct);

        var accessToken = jwt.GenerateAccessToken(user);
        return new AuthResponseDto(accessToken, newRefreshToken,
            new UserInfoDto(user.Id, user.Email, user.FirstName, user.LastName, user.MainPhoto?.Url));
    }
}
