using Microsoft.AspNetCore.SignalR;

namespace web.SignalR.Hubs
{
    public interface INotificationHubClient
    {
        Task SendAlert(string message, string detailuri, string confirmuri);
    }

    public class NotificationHub : Hub<INotificationHubClient>
    {
        public async Task SendAlert(string message, string detailUri, string confirmUri)
        {
            await Clients.Others.SendAlert(message, detailUri, confirmUri);
        }
    }
}
