using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class PatientIntake
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int MedicineId { get; set; }

        public TimeOnly IntakeStart { get; set; }

        public TimeOnly IntakeEnd { get; set; }

        public int Amount { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
