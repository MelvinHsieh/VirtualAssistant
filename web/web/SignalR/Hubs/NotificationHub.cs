using Microsoft.AspNetCore.SignalR;

namespace web.SignalR.Hubs
{
    public interface INotificationHubClient
    {
        Task SendAlert(string message);
    }

    public class NotificationHub : Hub<INotificationHubClient>
    {
        public async Task SendAlert (string message)
        {
            await Clients.Others.SendAlert(message);
        }
    }
}
