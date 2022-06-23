using Microsoft.AspNetCore.SignalR;

namespace web.SignalR.Hubs
{
    public interface INotificationHubClient
    {
        Task SendAlert(string message, string URI);
    }

    public class NotificationHub : Hub<INotificationHubClient>
    {
        public async Task SendAlert(string message, string URI)
        {
            await Clients.Others.SendAlert(message, URI);
        }
    }
}
