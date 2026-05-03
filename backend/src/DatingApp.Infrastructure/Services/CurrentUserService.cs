using DatingApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DatingApp.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var value = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return value is not null ? Guid.Parse(value) : null;
        }
    }
    public bool IsAuthenticated => accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
