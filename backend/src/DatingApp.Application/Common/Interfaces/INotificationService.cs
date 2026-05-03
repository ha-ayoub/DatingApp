namespace DatingApp.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendMatchNotificationAsync(Guid userId, Guid matchId, CancellationToken ct = default);
    Task SendMessageNotificationAsync(Guid userId, Guid matchId, string preview, CancellationToken ct = default);
}
