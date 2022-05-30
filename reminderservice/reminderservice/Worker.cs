using System.Net.Http;
using System.Net.Http.Json;

namespace ReminderService
{
    public class Worker : BackgroundService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<Worker> _logger;
        private readonly Uri _loginURI;
        private readonly Uri _dataURI;

        public Worker(ILogger<Worker> logger, IHttpClientFactory factory, IConfiguration configuration)
        {
            _logger = logger;
            _clientFactory = factory;
            _loginURI = new Uri(configuration.GetValue<string>("AuthURL") + "/login");
            _dataURI = new Uri(configuration.GetValue<string>("DataserviceURL") + "/");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("\n\n");
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                //Get an authToken from auth
                var authClient = _clientFactory.CreateClient("auth");
                var authResponse = authClient.PostAsJsonAsync(_loginURI, "{ \"username\": \"admin1\", \"password\": \"admin!1\" }").Result; //TODO extract (worker login?)
                var token = await authResponse.Content.ReadAsStringAsync();

                //if token, perform data operations
                if(!string.IsNullOrEmpty(token)) { 
                    var client = _clientFactory.CreateClient("client");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var response = await client.GetAsync("");

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("API response: Succes (Void)");
                    }
                    else
                    {
                        _logger.LogError("API returned: {statusCode}", response.StatusCode);
                    }                
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}