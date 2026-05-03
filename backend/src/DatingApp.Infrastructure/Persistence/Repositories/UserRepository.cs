using DatingApp.Domain.Entities;
using DatingApp.Domain.Enums;
using DatingApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    => db.Users
        .Include(u => u.Photos.OrderBy(p => p.Order))
        .FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => db.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        => db.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);

    public async Task<IEnumerable<User>> GetDiscoverableUsersAsync(
        Guid currentUserId, int minAge, int maxAge, List<Gender> genderPreferences,
        int maxDistanceKm, double? lat, double? lng, int skip, int take, CancellationToken ct = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var minDob = today.AddYears(-maxAge);
        var maxDob = today.AddYears(-minAge);

        var swipedIds = await db.Swipes
            .Where(s => s.SwiperId == currentUserId)
            .Select(s => s.SwipedId).ToListAsync(ct);

        var blockedByIds = await db.UserBlocks
            .Where(b => b.BlockedId == currentUserId)
            .Select(b => b.BlockerId).ToListAsync(ct);

        var query = db.Users
            .Include(u => u.Photos)
            .Where(u => u.Id != currentUserId
                && u.IsActive
                && !swipedIds.Contains(u.Id)
                && !blockedByIds.Contains(u.Id)
                && u.DateOfBirth >= minDob
                && u.DateOfBirth <= maxDob);

        if (genderPreferences.Count > 0)
            query = query.Where(u => genderPreferences.Contains(u.Gender));

        return await query.OrderByDescending(u => u.LastActiveAt)
            .Skip(skip).Take(take).ToListAsync(ct);
    }

    public Task AddAsync(User user, CancellationToken ct = default)
        => db.Users.AddAsync(user, ct).AsTask();

    public void Update(User user) => db.Users.Update(user);
}
