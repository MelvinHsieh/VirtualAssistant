using Application.Common.Models;
using Domain.Entities.PatientData;

namespace Application.Repositories.Interfaces
{
    public interface IPatientRepo
    {
        public Result AddPatient(string firstname, string lastname, DateTime birthdate, string postalcode, string housenumber, string email, string phonenumber);

        public Patient? GetPatient(int id);

        public bool DoesPatientExist(int id);

        public Result RemovePatient(int id);

        public Result ValidatePatient(Patient patient);
    }
}
