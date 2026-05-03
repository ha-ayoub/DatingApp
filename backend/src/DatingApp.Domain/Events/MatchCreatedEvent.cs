using DatingApp.Domain.Common;
namespace DatingApp.Domain.Events;
public record MatchCreatedEvent(Guid MatchId, Guid User1Id, Guid User2Id) : IDomainEvent;
