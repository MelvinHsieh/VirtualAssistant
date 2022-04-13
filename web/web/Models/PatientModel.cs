using Newtonsoft.Json;
using System.ComponentModel;

namespace web.Models
{
    public class PatientModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } = 0;

        [JsonProperty(PropertyName = "firstName")]
        [DisplayName("Voornaam")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "lastName")]
        [DisplayName("Achternaam")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "email")]
        [DisplayName("E-mail")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "phoneNumber")]
        [DisplayName("Telefoon nummer")]
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "birthDate")]
        [DisplayName("Geboortedatum")]
        public string BirthDate { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "postalCode")]
        [DisplayName("Postcode")]
        public string PostalCode { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "homeNumber")]
        [DisplayName("Huisnummer")]
        public string HomeNumber { get; set; } = string.Empty;

    }
}
