namespace DatingApp.Application.Features.Auth.DTOs;

public record AuthResponseDto(string AccessToken, string RefreshToken, UserInfoDto User);
public record UserInfoDto(Guid Id, string Email, string FirstName, string LastName, string? MainPhotoUrl);
