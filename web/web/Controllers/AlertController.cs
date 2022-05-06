using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using web.SignalR.Hubs;

namespace web.Controllers
{
    public class AlertController : Controller
    {
        private readonly IHubContext<NotificationHub> _hub;

        public AlertController(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task<IActionResult> Index()
        {
            await _hub.Clients.All.SendAsync("AlertCaretaker", "ALERT");
            return Ok();
        }
    }
}
