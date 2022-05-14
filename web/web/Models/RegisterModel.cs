using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class RegisterModel
    {
        [Required]
        [JsonProperty(PropertyName = "firstName")]
        [DisplayName("Gebruikersnaam")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [JsonProperty(PropertyName = "lastName")]
        [DisplayName("Wachtwoord")]
        public string LastName { get; set; } = string.Empty;

    }
}
