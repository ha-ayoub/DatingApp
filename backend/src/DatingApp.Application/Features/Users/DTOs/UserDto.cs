using DatingApp.Domain.Enums;

namespace DatingApp.Application.Features.Users.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string? Bio { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public Gender Gender { get; set; }
    public string? MainPhotoUrl { get; set; }
    public List<PhotoDto> Photos { get; set; } = [];
    public DateTime? LastActiveAt { get; set; }
}

public class UserProfileDto : UserDto
{
    public int MinAgePreference { get; set; }
    public int MaxAgePreference { get; set; }
    public int MaxDistanceKm { get; set; }
    public List<Gender> GenderPreferences { get; set; } = [];
}

public class PhotoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public int Order { get; set; }
}
