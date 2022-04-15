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
            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://{configuration["DataServiceHostName"]}:{configuration["DataServicePort"]}/api/");
        }

        public async Task<string> GetRequest(string endpoint)
        {
            return await httpClient.GetStringAsync(endpoint);
        }
    }
}
