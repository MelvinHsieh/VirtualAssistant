using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class IntakeModel
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } = 0;

        [Required]
        [JsonProperty(PropertyName = "patientId")]
        public int PatientId { get; set; } = 0;

        [Required]
        [JsonProperty(PropertyName = "medicineId")]
        public int MedicineId { get; set; } = 0;

        [Required]
        [JsonProperty(PropertyName = "intakeStart")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime IntakeStart { get; set; } = DateTime.Now;

        [Required]
        [JsonProperty(PropertyName = "intakeEnd")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime IntakeEnd { get; set; } = DateTime.Now;

        [Required]
        [JsonProperty(PropertyName = "amount")]
        [Range(1, 100)]
        [DisplayName("Aantal")]
        public int Amount { get; set; } = 0;

    }
}
