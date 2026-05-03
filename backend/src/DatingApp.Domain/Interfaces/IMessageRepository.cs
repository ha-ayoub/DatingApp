using DatingApp.Domain.Entities;
namespace DatingApp.Domain.Interfaces;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMatchMessagesAsync(Guid matchId, int skip, int take, CancellationToken ct = default);
    Task AddAsync(Message message, CancellationToken ct = default);
    void Update(Message message);
}
