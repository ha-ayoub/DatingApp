using AutoMapper;
using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Matches.DTOs;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Matches.Queries;

public record GetUserMatchesQuery(int Page = 1, int PageSize = 20) : IRequest<List<MatchDto>>;

public class GetUserMatchesQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    : IRequestHandler<GetUserMatchesQuery, List<MatchDto>>
{
    public async Task<List<MatchDto>> Handle(GetUserMatchesQuery request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();
        var skip = (request.Page - 1) * request.PageSize;
        var matches = await uow.Matches.GetUserMatchesAsync(userId, skip, request.PageSize, ct);
        return mapper.Map<List<MatchDto>>(matches);
    }
}
