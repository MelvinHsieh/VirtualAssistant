using Microsoft.AspNetCore.SignalR;

namespace web.SignalR.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task AlertCaretaker(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveCaretakerAlert", user, message);
        }
    }
}
