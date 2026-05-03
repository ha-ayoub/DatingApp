namespace DatingApp.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IMatchRepository Matches { get; }
    IMessageRepository Messages { get; }
    ISwipeRepository Swipes { get; }
    IPhotoRepository Photos { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
