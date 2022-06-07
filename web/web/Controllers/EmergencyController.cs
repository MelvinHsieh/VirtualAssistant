using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using web.Utils;

namespace web.Controllers
{
    public class EmergencyController : Controller
    {
        private readonly string _apiURL;

        public EmergencyController(IConfiguration configuration)
        {
            _apiURL = configuration.GetValue<String>("DataServiceURL");
        }

        [HttpPut("/confirm/{id}")]
        public async Task ConfirmAlert(int id)
        {
            //TODO
            // details van patient locatie laten zien
            // in de dataservice confirmen 

            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/confirm");
                    var body = new
                    {
                        patientId = id
                    };

                    StringContent content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                    var response = client.PutAsync(uri, content).Result;

                    string result = await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                TempData["error"] = "Geen connectie kon gemaakt worden met de Dataservice.";
            }
        }
    }
}
