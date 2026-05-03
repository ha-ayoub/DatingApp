using DatingApp.Domain.Entities;
namespace DatingApp.Domain.Interfaces;

public interface ISwipeRepository
{
    Task<Swipe?> GetAsync(Guid swiperId, Guid swipedId, CancellationToken ct = default);
    Task AddAsync(Swipe swipe, CancellationToken ct = default);
}
