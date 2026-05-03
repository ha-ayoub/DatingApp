using DatingApp.Domain.Interfaces;

namespace DatingApp.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext db,
    IUserRepository users, IMatchRepository matches,
    IMessageRepository messages, ISwipeRepository swipes,
    IPhotoRepository photos) : IUnitOfWork
{
    public IUserRepository Users => users;
    public IMatchRepository Matches => matches;
    public IMessageRepository Messages => messages;
    public ISwipeRepository Swipes => swipes;
    public IPhotoRepository Photos => photos;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
