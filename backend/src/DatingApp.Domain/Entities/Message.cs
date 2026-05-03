using DatingApp.Domain.Common;

namespace DatingApp.Domain.Entities;

public class Message : BaseEntity
{
    private Message() { }

    public Guid MatchId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }

    public Match Match { get; private set; } = null!;
    public User Sender { get; private set; } = null!;

    public static Message Create(Guid matchId, Guid senderId, string content)
        => new() { MatchId = matchId, SenderId = senderId, Content = content };

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
        SetUpdated();
    }
}
