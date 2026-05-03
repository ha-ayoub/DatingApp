using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Auth.DTOs;
using DatingApp.Domain.Entities;
using DatingApp.Domain.Enums;
using DatingApp.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DatingApp.Application.Features.Auth.Commands;

public record RegisterCommand(
    string Email, string Password, string FirstName, string LastName,
    DateOnly DateOfBirth, Gender Gender) : IRequest<AuthResponseDto>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(64)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth).NotEmpty()
            .Must(dob => DateOnly.FromDateTime(DateTime.UtcNow).Year - dob.Year >= 18)
            .WithMessage("You must be at least 18 years old.");
    }
}

public class RegisterCommandHandler(IUnitOfWork uow, IJwtService jwt) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existing = await uow.Users.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new ConflictException("Email already registered.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(request.Email, passwordHash, request.FirstName, request.LastName, request.DateOfBirth, request.Gender);

        var refreshToken = jwt.GenerateRefreshToken();
        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(30));

        await uow.Users.AddAsync(user, ct);
        await uow.SaveChangesAsync(ct);

        var accessToken = jwt.GenerateAccessToken(user);
        return new AuthResponseDto(accessToken, refreshToken,
            new UserInfoDto(user.Id, user.Email, user.FirstName, user.LastName, null));
    }
}
