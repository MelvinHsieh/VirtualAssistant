using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class LoginModel
    {
        [Required]
        [JsonProperty(PropertyName = "username")]
        [DisplayName("Gebruikersnaam")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [JsonProperty(PropertyName = "password")]
        [DisplayName("Wachtwoord")]
        public string LastName { get; set; } = string.Empty;

    }
}
