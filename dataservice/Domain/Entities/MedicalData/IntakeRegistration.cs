using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class IntakeRegistration
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public int IntakeId { get; set; }
        [Required]
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();

        public virtual PatientIntake PatientIntake { get; set; }
    }
}
