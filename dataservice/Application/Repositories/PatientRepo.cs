using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.PatientData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class PatientRepo : IPatientRepo
    {
        private PatientDbContext _context { get; set; }

        public PatientRepo(PatientDbContext context)
        {
            _context = context;
        }

        public Result AddPatient(string firstname, string lastname, DateTime birthdate, string postalcode, string housenumber, string email, string phonenumber)
        {
            Patient patient = new Patient()
            {
                FirstName = firstname,
                LastName = lastname,
                PhoneNumber = phonenumber,
                Email = email,
                BirthDate = birthdate,
                PostalCode = postalcode,
                HomeNumber = housenumber,
            };

            Result result = ValidatePatient(patient);

            if (result.Success == false)
            {
                return result;
            }

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return new Result(true);
        }

        public bool DoesPatientExist(int id)
        {
            return _context.Patients.Any(p => p.Id == id);
        }

        public Patient? GetPatient(int id)
        {
            return _context.Patients.Find(id);
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _context.Patients;
        }

        public Result RemovePatient(int id)
        {
            Patient? patient = GetPatient(id);
            if (patient == null)
            {
                return new Result(false, "No patient with given id has been found");
            }

            _context.Remove(patient);
            _context.SaveChanges();
            return new Result(true);
        }

        public Result ValidatePatient(Patient patient)
        {
            if (string.IsNullOrEmpty(patient.FirstName) || string.IsNullOrEmpty(patient.LastName))
            {
                return new Result(false, "First/Lastname should not be empty");
            }

            if (patient.BirthDate.Equals(new DateTime()))
            {
                return new Result(false, "BirthDate should not be empty");
            }

            if (string.IsNullOrEmpty(patient.Email))
            {
                return new Result(false, "Email should not be empty");
            }
            if (string.IsNullOrEmpty(patient.PhoneNumber))
            {
                return new Result(false, "Phonenumber should not be empty");
            }
            if (string.IsNullOrEmpty(patient.PostalCode))
            {
                return new Result(false, "PostalCode should not be empty");
            }
            if (string.IsNullOrEmpty(patient.HomeNumber))
            {
                return new Result(false, "Housenumber should not be empty");
            }

            return new Result(true);
        }
    }
}
