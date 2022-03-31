using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IPatientIntakeRepo
    {
        public Result AddIntake(int medicineId, int patientId, int amount, TimeOnly start, TimeOnly end);

        public IEnumerable<PatientIntake> GetIntakesByPatientId(int patientId);
        public PatientIntake? GetIntakeById(int intakeId);

        public Result RemoveIntake(int id);

        public Result ValidateIntake(PatientIntake patientIntake);
    }
}
