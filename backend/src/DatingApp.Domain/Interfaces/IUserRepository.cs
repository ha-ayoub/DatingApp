using DatingApp.Domain.Entities;
using DatingApp.Domain.Enums;

namespace DatingApp.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task<IEnumerable<User>> GetDiscoverableUsersAsync(Guid currentUserId, int minAge, int maxAge,
        List<Gender> genderPreferences, int maxDistanceKm, double? lat, double? lng,
        int skip, int take, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    void Update(User user);
}
