using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Domain.Entities.PatientData;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public Result AddPatient(string firstname, string lastname, DateTime birthdate, string postalcode, string homenumber, string email, string phonenumber, string roomId)
        {
            PatientLocation location = new PatientLocation()
            {
                RoomId = roomId,
            };

            var locationResult = _context.PatientLocations.Add(location).Entity; 

            Patient patient = new Patient()
            {
                FirstName = firstname,
                LastName = lastname,
                PhoneNumber = phonenumber,
                Email = email,
                BirthDate = birthdate,
                PostalCode = postalcode,
                HomeNumber = homenumber,
                LocationId = locationResult.Id
            };

            Result result = ValidatePatient(patient);

            if (result.Success == false)
            {
                return result;
            }

            var entityEntry = _context.Patients.Add(patient);
            _context.SaveChanges();

            return new Result(true, null, entityEntry.Entity.Id);
        }

        public bool DoesPatientExist(int id)
        {
            return _context.Patients.Any(p => p.Id == id);
        }

        public Patient? GetPatient(int id)
        {
            return _context.Patients.Include(p => p.Location).FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _context.Patients.Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower());
        }

        public Result UpdatePatient(int id, string firstname, string lastname, DateTime birthdate, string postalcode, string homenumber, string email, string phonenumber, int careworkerid, string roomId)
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

            PatientLocation ? location = _context.PatientLocations.Find(patient.LocationId);
            if (location is null)
            {
                result.Success = false;
                result.Message = "De locatie bestaat niet.";
                return result;
            }

            location.RoomId = roomId;

            patient.FirstName = firstname;
            patient.LastName = firstname;
            patient.BirthDate = birthdate;
            patient.PostalCode = postalcode;
            patient.HomeNumber = homenumber;
            patient.PhoneNumber = phonenumber;
            patient.CareWorkerId = careworkerid;

            _context.PatientLocations.Update(location);
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

        public Result RegisterAlert(int id)
        {
            Result result = new Result(false, "Alert opslaan mislukt!");

            if (!DoesPatientExist(id))
            {
                result.Success = false;
                result.Message = "De patient bestaat niet.";
                return result;
            }

            Patient patient = GetPatient(id);

            patient.EmergencyNotices.Add(new EmergencyNotice
            {
                PatientId = id,
                Sent = DateTime.Now
            });

            _context.Patients.Update(patient);
            _context.SaveChanges();

            result.Success = true;
            result.Message = "De melding is geregistreerd!";

            return result;
        }

        public Result ConfirmAlert(int patientId) 
        {
            Result result = new Result(false, "Het bevestigen van de noodoproep is mislukt!");

            if (!DoesPatientExist(patientId))
            {
                result.Success = false;
                result.Message = "De patient bestaat niet.";
                return result;
            }
            
            EmergencyNotice? emergencynotice = _context.EmergencyNotices
                .Where(en => en.PatientId == patientId)
                .Where(en => en.Confirmed != true)
                .FirstOrDefault();

            if (emergencynotice is null) 
            {
                result.Success = false;
                result.Message = "Er is geen noodoproep geplaatst voor deze patient.";
                return result;
            }

            emergencynotice.Confirmed = true;

            _context.EmergencyNotices.Update(emergencynotice);
            _context.SaveChanges();

            result.Success = true;
            result.Message = "De noodoproep is bevestigd";

            return result;
        }
    }
}
