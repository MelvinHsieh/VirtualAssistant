using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

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
                        /*var end = DateTime.Now;
                        var start = end.AddMinutes(INTERVAL_MINUTES * -1);*/

                        var start = DateTime.Parse("2022-06-03T23:49:00");
                        var end = DateTime.Parse("2022-06-04T00:10:00");


                        var response = await _client.GetAsync(_dataURI + $"PatientIntake/missed?searchStart={start.ToString("s")}&searchEnd={end.ToString("s")}");

                        if (response.IsSuccessStatusCode)
                        {
                            //ForEach PatientId send to FIREBASE
                            var json = await response.Content.ReadAsStringAsync();    
                            var jtoken = JToken.Parse(json);
                        
                            foreach(JProperty x in jtoken)
                            {
                                //SEND DATA
                            }

                            _logger.LogInformation("API response: Succes (Void)");
                        }
                        else
                        {
                            { "1":[{ "id":1,"patientId":1,"medicineId":1,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":1,"name":"Rosuvastatine","indication":"Hypercholesterolemie","dose":10,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":3,"patientId":1,"medicineId":2,"intakeStart":"12:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":2,"name":"Hydrochloorthiazide","indication":"Hypertensei","dose":12.5,"doseUnit":"mg","shape":"Rond","color":"Rood","type":"Tablet","status":"active"},"status":"active"},{ "id":4,"patientId":1,"medicineId":3,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":3,"name":"Metformine","indication":"Diabetes Mellitus type 2","dose":500,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":5,"patientId":1,"medicineId":4,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":4,"name":"Pantroprazol msr","indication":"Maagbeschermer","dose":80,"doseUnit":"mg","shape":"Vierkant","color":"Blauw","type":"Tablet","status":"active"},"status":"active"},{ "id":6,"patientId":1,"medicineId":5,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":5,"name":"Nitrofurantione","indication":"Antibiotica (Urineweginfectie)","dose":50,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":7,"patientId":1,"medicineId":6,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":6,"name":"Temazepam","indication":"Somberheid","dose":10,"doseUnit":"mg","shape":"Rond","color":"Groen","type":"Tablet","status":"active"},"status":"active"},{ "id":8,"patientId":1,"medicineId":7,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":7,"name":"Furosemide","indication":"Hartfalen","dose":40,"doseUnit":"mg","shape":"Hexagonaal","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":9,"patientId":1,"medicineId":8,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":8,"name":"Finasteride","indication":"Nycturie","dose":5,"doseUnit":"mg","shape":"Rond","color":"Zwart","type":"Capsule","status":"active"},"status":"active"},{ "id":10,"patientId":1,"medicineId":9,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":9,"name":"Oxazepam","indication":"Slaapproblemen","dose":10,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"}],"2":[{ "id":1,"patientId":1,"medicineId":1,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":1,"name":"Rosuvastatine","indication":"Hypercholesterolemie","dose":10,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":3,"patientId":1,"medicineId":2,"intakeStart":"12:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":2,"name":"Hydrochloorthiazide","indication":"Hypertensei","dose":12.5,"doseUnit":"mg","shape":"Rond","color":"Rood","type":"Tablet","status":"active"},"status":"active"},{ "id":4,"patientId":1,"medicineId":3,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":3,"name":"Metformine","indication":"Diabetes Mellitus type 2","dose":500,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":5,"patientId":1,"medicineId":4,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":4,"name":"Pantroprazol msr","indication":"Maagbeschermer","dose":80,"doseUnit":"mg","shape":"Vierkant","color":"Blauw","type":"Tablet","status":"active"},"status":"active"},{ "id":6,"patientId":1,"medicineId":5,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":5,"name":"Nitrofurantione","indication":"Antibiotica (Urineweginfectie)","dose":50,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":7,"patientId":1,"medicineId":6,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":6,"name":"Temazepam","indication":"Somberheid","dose":10,"doseUnit":"mg","shape":"Rond","color":"Groen","type":"Tablet","status":"active"},"status":"active"},{ "id":8,"patientId":1,"medicineId":7,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":7,"name":"Furosemide","indication":"Hartfalen","dose":40,"doseUnit":"mg","shape":"Hexagonaal","color":"Wit","type":"Tablet","status":"active"},"status":"active"},{ "id":9,"patientId":1,"medicineId":8,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":8,"name":"Finasteride","indication":"Nycturie","dose":5,"doseUnit":"mg","shape":"Rond","color":"Zwart","type":"Capsule","status":"active"},"status":"active"},{ "id":10,"patientId":1,"medicineId":9,"intakeStart":"00:00:00.000","intakeEnd":"23:59:59.000","amount":1,"medicine":{ "id":9,"name":"Oxazepam","indication":"Slaapproblemen","dose":10,"doseUnit":"mg","shape":"Vierkant","color":"Wit","type":"Tablet","status":"active"},"status":"active"}]}
                            _logger.LogError("API returned: {statusCode}", response.StatusCode);
                        }                
                    }
                }

                await Task.Delay(15000, stoppingToken);
            }
        }
    }
}