using AutoMapper;
using DatingApp.Application.Common.Exceptions;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Features.Users.DTOs;
using DatingApp.Domain.Interfaces;
using MediatR;

namespace DatingApp.Application.Features.Users.Queries;

public record GetDiscoverUsersQuery(int Page = 1, int PageSize = 10) : IRequest<List<UserDto>>;

public class GetDiscoverUsersQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser, IMapper mapper)
    : IRequestHandler<GetDiscoverUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetDiscoverUsersQuery request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();
        var user = await uow.Users.GetByIdAsync(userId, ct) ?? throw new NotFoundException("User", userId);

        var skip = (request.Page - 1) * request.PageSize;
        var users = await uow.Users.GetDiscoverableUsersAsync(
            userId, user.MinAgePreference, user.MaxAgePreference,
            user.GenderPreferences, user.MaxDistanceKm, user.Latitude, user.Longitude,
            skip, request.PageSize, ct);

        return mapper.Map<List<UserDto>>(users);
    }
}
