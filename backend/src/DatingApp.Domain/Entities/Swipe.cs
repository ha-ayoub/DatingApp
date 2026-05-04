using DatingApp.Domain.Common;
using DatingApp.Domain.Enums;

namespace DatingApp.Domain.Entities;

public class Swipe : BaseEntity
{
    public Swipe() { }

    public Guid SwiperId { get; set; }
    public Guid SwipedId { get; set; }
    public SwipeDirection Direction { get; set; }

    public User Swiper { get; set; } = null!;
    public User Swiped { get; set; } = null!;

    public static Swipe Create(Guid swiperId, Guid swipedId, SwipeDirection direction)
        => new() { SwiperId = swiperId, SwipedId = swipedId, Direction = direction };
}
