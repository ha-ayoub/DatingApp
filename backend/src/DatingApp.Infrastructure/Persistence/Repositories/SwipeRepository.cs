using DatingApp.Domain.Entities;
using DatingApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Repositories;

public class SwipeRepository(AppDbContext db) : ISwipeRepository
{
    public Task<Swipe?> GetAsync(Guid swiperId, Guid swipedId, CancellationToken ct = default)
        => db.Swipes.FirstOrDefaultAsync(s => s.SwiperId == swiperId && s.SwipedId == swipedId, ct);

    public Task AddAsync(Swipe swipe, CancellationToken ct = default)
        => db.Swipes.AddAsync(swipe, ct).AsTask();
}
