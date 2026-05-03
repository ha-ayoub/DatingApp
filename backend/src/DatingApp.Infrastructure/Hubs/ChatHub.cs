using DatingApp.Application.Features.Messages.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.Infrastructure.Hubs;

[Authorize]
public class ChatHub(IMediator mediator) : Hub
{
    public Task JoinMatch(string matchId)
        => Groups.AddToGroupAsync(Context.ConnectionId, matchId);

    public Task LeaveMatch(string matchId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);

    public async Task SendMessage(Guid matchId, string content)
    {
        var message = await mediator.Send(new SendMessageCommand(matchId, content));
        await Clients.Group(matchId.ToString()).SendAsync("ReceiveMessage", message);
    }

    public override Task OnConnectedAsync()
        => Clients.Caller.SendAsync("Connected", Context.ConnectionId);
}
