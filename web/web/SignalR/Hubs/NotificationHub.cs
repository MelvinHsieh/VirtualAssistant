using Microsoft.AspNetCore.SignalR;
using web.Models;

namespace web.SignalR.Hubs
{
    public interface INotificationHubClient
    {
        Task SendAlert(string message);
    }

    public class NotificationHub : Hub<INotificationHubClient>
    {
        public async Task SendAlert (string alertJson)
        {
            await Clients.Others.SendAlert(alertJson);
        }
    }
}
