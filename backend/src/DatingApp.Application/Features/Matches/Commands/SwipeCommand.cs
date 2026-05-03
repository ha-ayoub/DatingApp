using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Entities;
using DatingApp.Domain.Enums;
using DatingApp.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DatingApp.Application.Features.Matches.Commands;

public record SwipeCommand(Guid SwipedUserId, SwipeDirection Direction) : IRequest<SwipeResultDto>;
public record SwipeResultDto(bool IsMatch, Guid? MatchId);

public class SwipeCommandValidator : AbstractValidator<SwipeCommand>
{
    public SwipeCommandValidator()
    {
        RuleFor(x => x.SwipedUserId).NotEmpty();
        RuleFor(x => x.Direction).IsInEnum();
    }
}

public class SwipeCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser, INotificationService notification)
    : IRequestHandler<SwipeCommand, SwipeResultDto>
{
    public async Task<SwipeResultDto> Handle(SwipeCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();

        var existing = await uow.Swipes.GetAsync(userId, request.SwipedUserId, ct);
        if (existing is not null) return new SwipeResultDto(false, null);

        var swipe = Swipe.Create(userId, request.SwipedUserId, request.Direction);
        await uow.Swipes.AddAsync(swipe, ct);

        if (request.Direction is SwipeDirection.Right or SwipeDirection.SuperLike)
        {
            var reverseSwipe = await uow.Swipes.GetAsync(request.SwipedUserId, userId, ct);
            if (reverseSwipe is not null && reverseSwipe.Direction is SwipeDirection.Right or SwipeDirection.SuperLike)
            {
                var existingMatch = await uow.Matches.GetByUsersAsync(userId, request.SwipedUserId, ct);
                if (existingMatch is null)
                {
                    var match = Match.Create(userId, request.SwipedUserId);
                    await uow.Matches.AddAsync(match, ct);
                    await uow.SaveChangesAsync(ct);
                    await notification.SendMatchNotificationAsync(request.SwipedUserId, match.Id, ct);
                    return new SwipeResultDto(true, match.Id);
                }
            }
        }

        await uow.SaveChangesAsync(ct);
        return new SwipeResultDto(false, null);
    }
}
