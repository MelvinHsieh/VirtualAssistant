using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class IntakeModel
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        [ScaffoldColumn(false)]
        public int Id { get; set; } = 0;

        [Required]
        [JsonProperty(PropertyName = "patientId")]
        [ScaffoldColumn(false)]
        public int PatientId { get; set; } = 0;

        [Required]
        [JsonProperty(PropertyName = "medicineId")]
        [ScaffoldColumn(false)]
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
        [DisplayName("Aantal")]
        public int Amount { get; set; } = 0;

    }
}
