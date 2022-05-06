using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using web.SignalR.Hubs;

namespace web.Controllers
{
    public class AlertController : Controller
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _hub;

        public AlertController(IHubContext<NotificationHub, INotificationHubClient> hub)
        {
            _hub = hub;
        }

        public async Task<IActionResult> Index()
        {
            await _hub.Clients.All.SendAlert("ALERT");  
            return Ok("Alert");
        }
    }
}
