using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.PatientData
{
    public class EmergencyNotice
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime Sent { get; set; }

        public bool Confirmed { get; set; } = false;

        [Required]
        public int PatientId{ get; set; }
        
        [Required]
        public Patient Patient { get; set; }
    }
}
