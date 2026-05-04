using DatingApp.Domain.Common;
using DatingApp.Domain.Events;

namespace DatingApp.Domain.Entities;

public class Match : BaseEntity
{
    public Match() { }

    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    public bool IsActive { get; set; } = true;

    public User User1 { get; set; } = null!;
    public User User2 { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = [];

    public static Match Create(Guid user1Id, Guid user2Id)
    {
        var match = new Match { User1Id = user1Id, User2Id = user2Id };
        match.AddDomainEvent(new MatchCreatedEvent(match.Id, user1Id, user2Id));
        return match;
    }

    public void Unmatch() { IsActive = false; SetUpdated(); }
}
