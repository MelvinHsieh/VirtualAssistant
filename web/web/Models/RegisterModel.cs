using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class RegisterModel
    {
        [Required]
        [JsonProperty(PropertyName = "username")]
        [DisplayName("Gebruikersnaam")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [JsonProperty(PropertyName = "password")]
        [DisplayName("Wachtwoord")]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required]
        [JsonIgnore]
        [DisplayName("Wachtwoord")]
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
