using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Users.DTOs;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Users.Queries;

public record GetMyProfileQuery : IRequest<MyProfileDto>;

public record PhotoDto(Guid Id, string Url, bool IsMain, int Order);

public record MyProfileDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? Bio,
    string? City,
    string? Country,
    List<int> GenderPreferences,
    int MinAgePreference,
    int MaxAgePreference,
    int MaxDistanceKm,
    string? MainPhotoUrl,
    List<PhotoDto> Photos  // ← nouveau
);

public class GetMyProfileQueryHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser) : IRequestHandler<GetMyProfileQuery, MyProfileDto>
{
    public async Task<MyProfileDto> Handle(GetMyProfileQuery request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException("Not authenticated");
        var user = await uow.Users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("User not found", userId);

        return new MyProfileDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Bio,
            user.City,
            user.Country,
            user.GenderPreferences.Select(g => (int)g).ToList(),
            user.MinAgePreference,
            user.MaxAgePreference,
            user.MaxDistanceKm,
            user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
            // ← ajouter la liste complète des photos
            user.Photos.OrderBy(p => p.Order).Select(p => new PhotoDto(p.Id, p.Url, p.IsMain, p.Order)).ToList()
        );
    }
}