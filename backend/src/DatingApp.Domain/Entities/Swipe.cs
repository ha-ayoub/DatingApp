using DatingApp.Domain.Common;
using DatingApp.Domain.Enums;

namespace DatingApp.Domain.Entities;

public class Swipe : BaseEntity
{
    private Swipe() { }

    public Guid SwiperId { get; private set; }
    public Guid SwipedId { get; private set; }
    public SwipeDirection Direction { get; private set; }

    public User Swiper { get; private set; } = null!;
    public User Swiped { get; private set; } = null!;

    public static Swipe Create(Guid swiperId, Guid swipedId, SwipeDirection direction)
        => new() { SwiperId = swiperId, SwipedId = swipedId, Direction = direction };
}
