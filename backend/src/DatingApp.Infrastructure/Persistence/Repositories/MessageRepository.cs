using DatingApp.Domain.Entities;
using DatingApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Repositories;

public class MessageRepository(AppDbContext db) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetMatchMessagesAsync(Guid matchId, int skip, int take, CancellationToken ct = default)
        => await db.Messages
            .Where(m => m.MatchId == matchId)
            .OrderByDescending(m => m.CreatedAt)
            .Skip(skip).Take(take)
            .ToListAsync(ct);

    public Task AddAsync(Message message, CancellationToken ct = default)
        => db.Messages.AddAsync(message, ct).AsTask();

    public void Update(Message message) => db.Messages.Update(message);
}
