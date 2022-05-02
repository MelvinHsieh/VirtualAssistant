using System;

namespace Domain.Entities.MedicalData
{
    public class PatientIntake
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int MedicineId { get; set; }

        public DateTime IntakeStart { get; set; }

        public DateTime IntakeEnd { get; set; }

        public int Amount { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
