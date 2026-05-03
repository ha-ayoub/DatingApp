using DatingApp.Domain.Common;
namespace DatingApp.Domain.Events;
public record UserCreatedEvent(Guid UserId) : IDomainEvent;
