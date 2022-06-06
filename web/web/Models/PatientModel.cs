using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class PatientModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } = 0;

        [Required]
        [JsonProperty(PropertyName = "firstName")]
        [DisplayName("Voornaam")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [JsonProperty(PropertyName = "lastName")]
        [DisplayName("Achternaam")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [JsonProperty(PropertyName = "email")]
        [DisplayName("E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [JsonProperty(PropertyName = "phoneNumber")]
        [DisplayName("Telefoon nummer")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [JsonProperty(PropertyName = "birthDate")]
        [DisplayName("Geboortedatum")]
        public string BirthDate { get; set; } = string.Empty;

        [Required]
        /*[RegularExpression("^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$")] <-- this regex is not working for some reason*/
        [JsonProperty(PropertyName = "postalCode")]
        [DisplayName("Postcode")]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [JsonProperty(PropertyName = "homeNumber")]
        [DisplayName("Huisnummer")]
        public string HomeNumber { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "CareWorkerId")]
        [DisplayName("Zorgmedewerker")]
        public int CareWorkerId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "roomId")]
        [DisplayName("Zorgmedewerker")]
        public string RoomId { get; set; } = string.Empty;
    }
}
