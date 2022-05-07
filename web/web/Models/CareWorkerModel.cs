using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class CareWorkerModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } = 0;

        [JsonProperty(PropertyName = "firstName")]
        [DisplayName("Voornaam")]
        [Required]
        public string FirstName { get; set; } = "";

        [JsonProperty(PropertyName = "lastName")]
        [DisplayName("Achternaam")]
        [Required]
        public string LastName { get; set; } = "";

        [JsonProperty(PropertyName = "function")]
        [DisplayName("Functie")]
        [Required]
        public string Function { get; set; } = "";
    }
}
