using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IIntakeRegistrationRepo
    {
        public Result AddIntakeRegistration(DateTime date, int patientIntakeId);

        public IntakeRegistration? GetIntakeRegistration(int id);

        public List<IntakeRegistration> GetIntakeRegistrationForPatient(int patientId); //For a patient

        public List<IntakeRegistration> GetIntakeRegistrationForDate(int patientId, DateOnly date); //For a patient

        public Result RemoveIntakeRegistration(int id);

        public Result ValidateIntakeRegistration(IntakeRegistration intakeRegistration);
    }
}
