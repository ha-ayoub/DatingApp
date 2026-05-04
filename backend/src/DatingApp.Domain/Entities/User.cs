using DatingApp.Domain.Common;
using DatingApp.Domain.Enums;
using DatingApp.Domain.Events;

namespace DatingApp.Domain.Entities;

public class User : BaseEntity
{
    public User() { } // EF Core

    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int MinAgePreference { get; set; } = 18;
    public int MaxAgePreference { get; set; } = 99;
    public int MaxDistanceKm { get; set; } = 100;
    public List<Gender> GenderPreferences { get; set; } = [];
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; }
    public DateTime? LastActiveAt { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

    public ICollection<Photo> Photos { get; set; } = [];
    public ICollection<Swipe> SentSwipes { get; set; } = [];
    public ICollection<Swipe> ReceivedSwipes { get; set; } = [];
    public ICollection<Match> Matches { get; set; } = [];
    public ICollection<UserBlock> BlockedUsers { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];

    public int Age => CalculateAge(DateOfBirth);
    public Photo? MainPhoto => Photos.FirstOrDefault(p => p.IsMain);

    public static User Create(string email, string passwordHash, string firstName, string lastName,
        DateOnly dateOfBirth, Gender gender)
    {
        var user = new User
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            Gender = gender
        };
        user.AddDomainEvent(new UserCreatedEvent(user.Id));
        return user;
    }

    public void UpdateProfile(string? bio, string? city, string? country, int minAge, int maxAge, int maxDistanceKm, List<Gender> preferences)
    {
        Bio = bio;
        City = city;
        Country = country;
        MinAgePreference = minAge;
        MaxAgePreference = maxAge;
        MaxDistanceKm = maxDistanceKm;
        GenderPreferences = preferences;
        SetUpdated();
    }

    public void UpdateLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        SetUpdated();
    }

    public void SetRefreshToken(string token, DateTime expiresAt)
    {
        RefreshToken = token;
        RefreshTokenExpiresAt = expiresAt;
        SetUpdated();
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
        SetUpdated();
    }

    public void SetLastActive() => LastActiveAt = DateTime.UtcNow;

    public void Deactivate()
    {
        IsActive = false;
        SetUpdated();
    }

    private static int CalculateAge(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dob.Year;
        if (dob > today.AddYears(-age)) age--;
        return age;
    }
}
