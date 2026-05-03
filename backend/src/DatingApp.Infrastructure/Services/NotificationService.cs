using DatingApp.Application.Common.Interfaces;
using DatingApp.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.Infrastructure.Services;

public class NotificationService(IHubContext<ChatHub> hubContext) : INotificationService
{
    public Task SendMatchNotificationAsync(Guid userId, Guid matchId, CancellationToken ct = default)
        => hubContext.Clients.User(userId.ToString()).SendAsync("NewMatch", new { matchId }, ct);

    public Task SendMessageNotificationAsync(Guid userId, Guid matchId, string preview, CancellationToken ct = default)
        => hubContext.Clients.User(userId.ToString()).SendAsync("NewMessage", new { matchId, preview }, ct);
}
