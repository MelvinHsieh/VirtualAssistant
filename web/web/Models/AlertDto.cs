using Newtonsoft.Json;

namespace web.Models
{
    public class AlertDto
    {
        [JsonProperty("patientId")]
        public int? PatientId { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}
