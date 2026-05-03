using DatingApp.Domain.Entities;
namespace DatingApp.Domain.Interfaces;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Match?> GetByUsersAsync(Guid user1Id, Guid user2Id, CancellationToken ct = default);
    Task<IEnumerable<Match>> GetUserMatchesAsync(Guid userId, int skip, int take, CancellationToken ct = default);
    Task AddAsync(Match match, CancellationToken ct = default);
    void Update(Match match);
}
