using DatingApp.Domain.Common;

namespace DatingApp.Domain.Entities;

public class Photo : BaseEntity
{
    public Photo() { }
    public Guid UserId { get; set; }
    public string PublicId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public int Order { get; set; }

    public User User { get; set; } = null!;

    public static Photo Create(Guid userId, string publicId, string url, bool isMain = false, int order = 0)
        => new() { UserId = userId, PublicId = publicId, Url = url, IsMain = isMain, Order = order };

    public void SetAsMain() { IsMain = true; SetUpdated(); }
    public void UnsetMain() { IsMain = false; SetUpdated(); }
}
