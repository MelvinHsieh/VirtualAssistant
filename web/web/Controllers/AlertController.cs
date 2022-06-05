using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using web.Models;
using web.SignalR.Hubs;
using web.Utils;

namespace web.Controllers
{
    public class AlertController : Controller
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _hub;

        public AlertController(IHubContext<NotificationHub, INotificationHubClient> hub)
        {
            _hub = hub;
        }

        [HttpPost("/alert")]
        public async Task<IActionResult> PostAlert([FromBody] AlertDto alert)
        {
            await _hub.Clients.All.SendAlert(alert.Message);
            return Ok("Alert sent!");
        }
    }
}
