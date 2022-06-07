using Application.Common.Models;
using Domain.Entities.PatientData;

namespace Application.Repositories.Interfaces
{
    public interface IPatientRepo
    {
        public Result AddPatient(string firstname, string lastname, DateTime birthdate, string postalcode, string housenumber, string email, string phonenumber, string roomId);

        public Patient? GetPatient(int id);

        public IEnumerable<Patient> GetAllPatients();
        public Result UpdatePatient(int id, string firstname, string lastname, DateTime birthdate, string postalcode, string housenumber, string email, string phonenumber, int careworkerid, string roomId);

        public Result RemovePatient(int id);

        public bool DoesPatientExist(int id);

        public Result ValidatePatient(Patient patient);

        public Result RegisterAlert(int id);
        public Result ConfirmAlert(int patientId);
    }
}
