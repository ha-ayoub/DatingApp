using DatingApp.Domain.Entities;
using DatingApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Repositories;

public class MatchRepository(AppDbContext db) : IMatchRepository
{
    public Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Matches
            .Include(m => m.User1).ThenInclude(u => u.Photos)
            .Include(m => m.User2).ThenInclude(u => u.Photos)
            .Include(m => m.Messages.OrderByDescending(msg => msg.CreatedAt).Take(1))
            .FirstOrDefaultAsync(m => m.Id == id, ct);

    public Task<Match?> GetByUsersAsync(Guid user1Id, Guid user2Id, CancellationToken ct = default)
        => db.Matches.FirstOrDefaultAsync(m =>
            (m.User1Id == user1Id && m.User2Id == user2Id) ||
            (m.User1Id == user2Id && m.User2Id == user1Id), ct);

    public async Task<IEnumerable<Match>> GetUserMatchesAsync(Guid userId, int skip, int take, CancellationToken ct = default)
    {
        return await db.Matches
            .Include(m => m.User1).ThenInclude(u => u.Photos)
            .Include(m => m.User2).ThenInclude(u => u.Photos)
            .Include(m => m.Messages.OrderByDescending(msg => msg.CreatedAt).Take(1))
            .Where(m => (m.User1Id == userId || m.User2Id == userId) && m.IsActive)
            .OrderByDescending(m => m.Messages.Max(msg => (DateTime?)msg.CreatedAt) ?? m.CreatedAt)
            .Skip(skip).Take(take)
            .ToListAsync(ct);
    }

    public Task AddAsync(Match match, CancellationToken ct = default)
        => db.Matches.AddAsync(match, ct).AsTask();

    public void Update(Match match) => db.Matches.Update(match);
}
