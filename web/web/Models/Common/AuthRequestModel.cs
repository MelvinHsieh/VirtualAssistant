using Newtonsoft.Json;

namespace web.Models.Common
{
    public class AuthRequestModel
    {
        [JsonProperty(PropertyName = "username")]
        public string? UserName { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string? Password { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string? Role { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }  
    }
}
