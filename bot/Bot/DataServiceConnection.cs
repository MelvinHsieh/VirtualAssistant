using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreBot
{
    public class DataServiceConnection
    {
        private HttpClient httpClient;

        public DataServiceConnection(IConfiguration configuration)
        {
            var adminToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyIjp7Il9pZCI6IjYyOTdhMDhmYzEwYTIxNjJmYjg0M2U4NCIsInVzZXJuYW1lIjoiYWRtaW4xIiwicm9sZSI6ImFkbWluIn0sImF1dGhfcm9sZSI6ImFkbWluIiwiaWF0IjoxNjU0MTU4ODU2LCJhdWQiOiJWQV9BdXRoQXVkaWVuY2UiLCJpc3MiOiJWQV9BdXRoSXNzdWVyIn0.NCsb6pLbEQVc4NRxrFyXnVvFpChSoP0MLfolVoGnTz0";
            this.httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + adminToken);
            httpClient.BaseAddress = new Uri($"https://{configuration["DataServiceHostName"]}/api/");
        }

        public async Task<string> GetRequest(string endpoint)
        {
            return await httpClient.GetStringAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostRequest(string endpoint, string data)
        {
            StringContent httpContent = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endpoint, httpContent); 
        }
    }
}
