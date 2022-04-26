using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IIntakeRegistrationRepo
    {
        public Result AddIntakeRegistration(DateOnly date, int patientIntakeId);

        public IntakeRegistration? GetIntakeRegistration(int id);

        public IEnumerable<IntakeRegistration> GetIntakeRegistrationForPatient(int patientId); //For a patient

        public IEnumerable<IntakeRegistration> GetIntakeRegistrationForDate(int patientId, DateOnly date); //For a patient

        public Result RemoveIntakeRegistration(int id);

        public Result ValidateIntakeRegistration(IntakeRegistration intakeRegistration);
    }
}
