using Domain.Entities.MedicalData;
using Domain.Entities.PatientData;

namespace dataservice.DTO
{
    public class IntakeReminderDto
    {
        public IEnumerable<PatientIntake>? PatientIntake { get; set; }

        public string? DeviceId { get; set; }
    }
}
