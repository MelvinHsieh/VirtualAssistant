using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class PatientIntakeRepo : IPatientIntakeRepo
    {
        private MedicineDbContext _medicineDbContext { get; set; }
        private IMedicineRepo _medicineRepo { get; set; }
        private IPatientRepo _patientRepo { get; set; }
        private IIntakeRegistrationRepo _intakeRegistrationRepo { get; set; }

        public PatientIntakeRepo(MedicineDbContext medicineDbContext, IMedicineRepo medicineRepo,
            IPatientRepo patientRepo, IIntakeRegistrationRepo intakeRegistrationRepo)
        {
            _medicineRepo = medicineRepo;
            _medicineDbContext = medicineDbContext;
            _patientRepo = patientRepo;
            _intakeRegistrationRepo = intakeRegistrationRepo;
        }

        public Result AddIntake(int medicineId, int patientId, int amount, TimeOnly start, TimeOnly end)
        {
            PatientIntake intake = new PatientIntake()
            {
                MedicineId = medicineId,
                PatientId = patientId,
                Amount = amount,
                IntakeStart = start,
                IntakeEnd = end
            };

            Result result = ValidateIntake(intake);

            if (result.Success == false)
            {
                return result;
            }

            _medicineDbContext.PatientIntakes.Add(intake);
            _medicineDbContext.SaveChanges();

            return new Result(true);
        }

        public PatientIntake? GetIntakeById(int intakeId)
        {
            return _medicineDbContext.PatientIntakes.Include(x => x.Medicine).First(x => x.Id == intakeId);
        }

        public IEnumerable<PatientIntake> GetIntakesByPatientId(int patientId)
        {
            return _medicineDbContext.PatientIntakes.Include(x => x.Medicine).Where(x => x.PatientId == patientId).Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower());
        }

        public IEnumerable<PatientIntake> GetRemainingIntakesByPatientId(int patientId)
        {
            IEnumerable<IntakeRegistration> intakeRegistrations = new List<IntakeRegistration>();
            intakeRegistrations = _intakeRegistrationRepo.GetIntakeRegistrationForDate(patientId, DateOnly.FromDateTime(DateTime.Now));

            return _medicineDbContext.PatientIntakes.Include(x => x.Medicine)
                .Where(x => x.PatientId == patientId)
                .Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower())
                .Where(y => !intakeRegistrations.Any(reg => reg.PatientIntakeId == y.Id));
        }

        public Result RemoveIntake(int id)
        {
            var foundIntake = GetIntakeById(id);
            if (foundIntake == null)
            {
                return new Result(false, "No intake with given id exists");
            }

            foundIntake.Status = Domain.EntityStatus.Archived.ToString().ToLower(); //Soft-Delete
            _medicineDbContext.PatientIntakes.Update(foundIntake);
            _medicineDbContext.SaveChanges();
            return new Result(true);
        }

        public Result ValidateIntake(PatientIntake patientIntake)
        {
            if (_medicineRepo.GetMedicine(patientIntake.MedicineId) == null)
            {
                return new Result(false, "The given medicine does not exist");
            }

            if (_patientRepo.DoesPatientExist(patientIntake.PatientId) == false)
            {
                return new Result(false, "The given patient does not exist");
            }

            if (patientIntake.Amount <= 0)
            {
                return new Result(false, "The given amount should at least be 1");
            }

            if (patientIntake.IntakeStart.Equals(patientIntake.IntakeEnd))
            {
                return new Result(false, "Intake start and intake end should not be equal");
            }

            return new Result(true);
        }
    }
}
