using AutoMapper;
using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Messages.DTOs;
using DatingApp.Domain.Entities;
using DatingApp.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DatingApp.Application.Features.Messages.Commands;

public record SendMessageCommand(Guid MatchId, string Content) : IRequest<MessageDto>;

public class SendMessageValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(1000);
    }
}

public class SendMessageCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    : IRequestHandler<SendMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();
        var match = await uow.Matches.GetByIdAsync(request.MatchId, ct)
            ?? throw new NotFoundException(nameof(Match), request.MatchId);

        if (match.User1Id != userId && match.User2Id != userId)
            throw new UnauthorizedException("You are not part of this match.");

        if (!match.IsActive) throw new ConflictException("Match is no longer active.");

        var message = Message.Create(request.MatchId, userId, request.Content);
        await uow.Messages.AddAsync(message, ct);
        await uow.SaveChangesAsync(ct);

        return mapper.Map<MessageDto>(message);
    }
}
