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
        public string Username { get; set; } = string.Empty;

        [Required]
        [PasswordPropertyText]
        [JsonProperty(PropertyName = "password")]
        [DisplayName("Wachtwoord")]
        public string Password { get; set; } = string.Empty;

    }
}
