using AutoMapper;
using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Messages.DTOs;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Messages.Queries;

public record GetMessagesQuery(Guid MatchId, int Page = 1, int PageSize = 30) : IRequest<List<MessageDto>>;

public class GetMessagesQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    : IRequestHandler<GetMessagesQuery, List<MessageDto>>
{
    public async Task<List<MessageDto>> Handle(GetMessagesQuery request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();
        var match = await uow.Matches.GetByIdAsync(request.MatchId, ct)
            ?? throw new NotFoundException("Match", request.MatchId);

        if (match.User1Id != userId && match.User2Id != userId)
            throw new UnauthorizedException();

        var skip = (request.Page - 1) * request.PageSize;
        var messages = await uow.Messages.GetMatchMessagesAsync(request.MatchId, skip, request.PageSize, ct);
        return mapper.Map<List<MessageDto>>(messages);
    }
}
