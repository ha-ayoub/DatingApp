using DatingApp.Domain.Common;
using DatingApp.Domain.Events;

namespace DatingApp.Domain.Entities;

public class Match : BaseEntity
{
    private Match() { }

    public Guid User1Id { get; private set; }
    public Guid User2Id { get; private set; }
    public bool IsActive { get; private set; } = true;

    public User User1 { get; private set; } = null!;
    public User User2 { get; private set; } = null!;
    public ICollection<Message> Messages { get; private set; } = [];

    public static Match Create(Guid user1Id, Guid user2Id)
    {
        var match = new Match { User1Id = user1Id, User2Id = user2Id };
        match.AddDomainEvent(new MatchCreatedEvent(match.Id, user1Id, user2Id));
        return match;
    }

    public void Unmatch() { IsActive = false; SetUpdated(); }
}
