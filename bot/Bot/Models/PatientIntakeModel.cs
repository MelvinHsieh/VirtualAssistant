using System;

namespace CoreBot.Models
{
    public class PatientIntakeModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int MedicineId { get; set; }
        public TimeSpan IntakeStart { get; set; }
        public TimeSpan IntakeEnd { get; set; }
        public int Amount { get; set; }
        public MedicineModel Medicine { get; set; }
        public string Status { get; set; }
    }
}
