using DatingApp.Domain.Common;

namespace DatingApp.Domain.Entities;

public class UserBlock : BaseEntity
{
    private UserBlock() { }

    public Guid BlockerId { get; private set; }
    public Guid BlockedId { get; private set; }

    public User Blocker { get; private set; } = null!;
    public User Blocked { get; private set; } = null!;

    public static UserBlock Create(Guid blockerId, Guid blockedId)
        => new() { BlockerId = blockerId, BlockedId = blockedId };
}
