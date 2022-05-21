using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Domain.Entities.PatientData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class PatientRepo : IPatientRepo
    {
        private PatientDbContext _context { get; set; }
        private ICareWorkerRepo _careWorkerRepo { get; set; }

        public PatientRepo(PatientDbContext context, ICareWorkerRepo careWorkerRepo)
        {
            _context = context;
            _careWorkerRepo = careWorkerRepo;
        }

        public Result AddPatient(string firstname, string lastname, DateTime birthdate, string postalcode, string homenumber, string email, string phonenumber)
        {
            Patient patient = new Patient()
            {
                FirstName = firstname,
                LastName = lastname,
                PhoneNumber = phonenumber,
                Email = email,
                BirthDate = birthdate,
                PostalCode = postalcode,
                HomeNumber = homenumber,
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
            return _context.Patients.Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower());
        }

        public Result UpdatePatient(int id, string firstname, string lastname, DateTime birthdate, string postalcode, string homenumber, string email, string phonenumber, int careworkerid)
        {
            Result result = new Result(false, "Patient aanpassen mislukt!");

            if (!DoesPatientExist(id))
            {
                return result;
            }

            Patient? patient = GetPatient(id);
            if (patient is null)
            {
                result.Success = false;
                result.Message = "De patient bestaat niet.";
                return result;
            }

            CareWorker? careWorker = _careWorkerRepo.FindCareWorker(careworkerid);
            if (careWorker is null && careworkerid != 0)
            {
                result.Success = false;
                result.Message = "De zorgmedewerker bestaat niet.";
                return result;
            }

            patient.FirstName = firstname;
            patient.LastName = firstname;
            patient.BirthDate = birthdate;
            patient.PostalCode = postalcode;
            patient.HomeNumber = homenumber;
            patient.PhoneNumber = phonenumber;
            patient.CareWorkerId = careworkerid;

            _context.Patients.Update(patient);
            _context.SaveChanges();

            result.Success = true;
            result.Message = "Patiënt aanpassen voltooid!";

            return result;
        }

        public Result RemovePatient(int id)
        {
            Patient? patient = GetPatient(id);
            if (patient == null)
            {
                return new Result(false, "No patient with given id has been found");
            }

            patient.Status = Domain.EntityStatus.Archived.ToString().ToLower(); //Soft-Delete
            _context.Patients.Update(patient);
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

        public Result RegisterAlert(int id, DateTime date)
        {
            Result result = new Result(false, "Alert opslaan mislukt!");

            if (!DoesPatientExist(id))
            {
                return result;
            }

            Patient? patient = GetPatient(id);
            if (patient is null)
            {
                result.Success = false;
                result.Message = "De patient bestaat niet.";
                return result;
            }

            patient.EmergencyNotices.Add(new EmergencyNotice {
                PatientId = id,
                Sent = date
            });

            _context.Patients.Update(patient);
            _context.SaveChanges();


            result.Success = true;
            result.Message = "De melding is geregistreerd!";

            return result;
        }
    }
}
