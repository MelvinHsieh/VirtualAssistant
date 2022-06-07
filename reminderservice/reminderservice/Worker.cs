using FirebaseAdmin.Messaging;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ReminderService
{
    public class Worker : BackgroundService
    {
        private HttpClient _client;
        private readonly ILogger<Worker> _logger;
        private readonly Uri _loginURI;
        private readonly Uri _dataURI;
        private const int INTERVAL_MINUTES = 15;

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
                _client.DefaultRequestHeaders.Remove("Authorization");
                //Get an authToken from auth
                var content = new StringContent("{ \"username\": \"admin1\", \"password\": \"admin!1\" }", UnicodeEncoding.UTF8, "application/json");
                var authResponse = await _client.PostAsync(_loginURI, content); //TODO extract (worker login?)

                if(authResponse.IsSuccessStatusCode) {  
                var token = await authResponse.Content.ReadAsStringAsync();

                    //if token, perform data operations
                    if(!string.IsNullOrEmpty(token)) { 
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JToken.Parse(token));


                        //Get DateTime strings 
                        var end = DateTime.Now;
                        var start = end.AddMinutes(INTERVAL_MINUTES * -1);

                        // Tester 
                        //var start = DateTime.Parse("2022-06-03T23:49:00");
                        //var end = DateTime.Parse("2022-06-04T00:10:00");


                        var response = await _client.GetAsync(_dataURI + $"PatientIntake/missed?searchStart={start.ToString("s")}&searchEnd={end.ToString("s")}");

                        if (response.IsSuccessStatusCode)
                        {
                            //ForEach PatientId send to FIREBASE
                            var json = await response.Content.ReadAsStringAsync();    

                            SendAllReminders(json);

                            _logger.LogInformation("API response: Succes (Void)");
                        }
                        else
                        {
                            _logger.LogError("API returned: {statusCode}", response.StatusCode);
                        }                
                    }
                }

                await Task.Delay(60000 * INTERVAL_MINUTES, stoppingToken);
            }
        }

        private async void SendAllReminders(string json)
        {
            var jtoken = JToken.Parse(json);
            var messages = new List<Message>();
            foreach (JProperty user in jtoken)
            {
                var deviceId = user.Children().First().Value<string>("deviceId");
                var intakeArray = user.Children().First().Value<JArray>("patientIntake");

                if(deviceId != null) {  
                    foreach (var intake in intakeArray)
                    {
                        var message = CreateMessage(int.Parse(user.Name), intake, deviceId);
                        if(message != null)
                        {
                            messages.Add(message);
                        }
                    }
                }
            }

            if(messages.Count > 0) { 
                var result = await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
            }
                return;
        }

        private Message? CreateMessage(int id, JToken intake, string deviceId)
        {
            string? medicineName = intake.Value<JToken>("medicine").Value<string>("name");
            TimeOnly intakeStart = TimeOnly.Parse(intake.Value<string>("intakeStart"));
            TimeOnly intakeEnd = TimeOnly.Parse(intake.Value<string>("intakeEnd"));

            if(medicineName != null) {  
                var message = new Message()
                {
                    Token = deviceId,
                    Notification = new Notification()
                    {
                        Title = "Gemiste inname",
                        Body = $"U heeft uw inname van {medicineName} tussen {intakeStart.ToString("HH:mm")} en {intakeEnd.ToString("HH:mm")} gemist"
                    }
                };
                return message;
            }

            return null;
        }
    }
}