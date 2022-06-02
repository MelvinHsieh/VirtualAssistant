using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ReminderService
{
    public class Worker : BackgroundService
    {
        private HttpClient _client;
        private readonly ILogger<Worker> _logger;
        private readonly Uri _loginURI;
        private readonly Uri _dataURI;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            _loginURI = new Uri(configuration.GetValue<string>("AuthURL") + "/login");
            _dataURI = new Uri(configuration.GetValue<string>("DataserviceURL") + "/");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {
                
                Console.WriteLine("\n\n");
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _client.DefaultRequestHeaders.Remove("Auhtorization");
                //Get an authToken from auth

                var authResponse = await _client.PostAsJsonAsync(_loginURI, "{ \"username\": \"admin1\", \"password\": \"admin!1\" }"); //TODO extract (worker login?)
                var token = await authResponse.Content.ReadAsStringAsync();

                //if token, perform data operations
                if(!string.IsNullOrEmpty(token)) { 
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var response = await _client.GetAsync(_dataURI + "PatientIntake/missed");

                    if (response.IsSuccessStatusCode)
                    {
                        //ForEach PatientId send to FIREBASE
                        var json = await response.Content.ReadAsStringAsync();    
                        var jtoken = JToken.Parse(json);
                        
                        foreach(JProperty x in jtoken)
                        {

                        }

                        _logger.LogInformation("API response: Succes (Void)");
                    }
                    else
                    {
                        _logger.LogError("API returned: {statusCode}", response.StatusCode);
                    }                
                }

                await Task.Delay(15000, stoppingToken);
            }
        }
    }
}