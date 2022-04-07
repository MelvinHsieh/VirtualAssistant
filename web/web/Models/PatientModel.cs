using Newtonsoft.Json;
using System.ComponentModel;

namespace web.Models
{
    public class PatientModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } = 0;

        [JsonProperty(PropertyName = "name")]
        [DisplayName("Naam")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "mail")]
        [DisplayName("E-mail")]
        public string mail { get; set; } = string.Empty;

    }
}
