using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using web.Models;
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

        [HttpPost("/alert")]
        public async Task<IActionResult> PostAlert([FromBody] AlertDto alert)
        {
            var alertJson = JsonConvert.SerializeObject(alert);
            await _hub.Clients.All.SendAlert(alertJson);  
            return Ok("Alert sent!");
        }
    }
}
