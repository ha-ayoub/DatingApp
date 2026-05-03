using BCrypt.Net;
using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Auth.DTOs;
using DatingApp.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DatingApp.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler(IUnitOfWork uow, IJwtService jwt) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await uow.Users.GetByEmailAsync(request.Email, ct)
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        if (!user.IsActive)
            throw new UnauthorizedException("Account deactivated.");

        var refreshToken = jwt.GenerateRefreshToken();
        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(30));
        user.SetLastActive();
        uow.Users.Update(user);
        await uow.SaveChangesAsync(ct);

        var accessToken = jwt.GenerateAccessToken(user);
        return new AuthResponseDto(accessToken, refreshToken,
            new UserInfoDto(user.Id, user.Email, user.FirstName, user.LastName, user.MainPhoto?.Url));
    }
}
