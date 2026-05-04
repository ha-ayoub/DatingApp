using DatingApp.Domain.Common;

namespace DatingApp.Domain.Entities;

public class Message : BaseEntity
{
    public Message() { }

    public Guid MatchId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    public Match Match { get; set; } = null!;
    public User Sender { get; set; } = null!;

    public static Message Create(Guid matchId, Guid senderId, string content)
        => new() { MatchId = matchId, SenderId = senderId, Content = content };

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
        SetUpdated();
    }
}
