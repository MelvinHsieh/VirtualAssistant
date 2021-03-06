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
            var adminToken = configuration["AdminToken"];
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
