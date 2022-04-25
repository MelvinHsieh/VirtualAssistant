using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class IntakeRegistrationRepo : IIntakeRegistrationRepo
    {
        private MedicineDbContext _context;
        private MedicineRepo _medicineRepo;

        public IntakeRegistrationRepo(MedicineRepo medicineRepo, MedicineDbContext context)
        {
            this._medicineRepo = medicineRepo;
            this._context = context;
        }

        public Result AddIntakeRegistration(DateOnly date, int patientIntakeId)
        {
            IntakeRegistration registration = new IntakeRegistration()
            {
                Date = date,
                Id = patientIntakeId
            };

            Result result = ValidateIntakeRegistration(registration);

            if (result.Success == false)
            {
                return result;
            }

            _context.IntakeRegistrations.Add(registration);
            _context.SaveChanges();

            return new Result(true);
        }

        public IntakeRegistration? GetIntakeRegistration(int id)
        {
            return _context.IntakeRegistrations.Include(x => x.PatientIntake).First(x => x.Id == id);
        }

        public IEnumerable<IntakeRegistration> GetPatientIntakeRegistrationForDate(int patientId, DateOnly date)
        {
            return _context.IntakeRegistrations
                .Include(x => x.PatientIntake)
                .Where(x => x.PatientIntake.PatientId == patientId)
                .Where(x => x.Date == date)
                .Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower());
        }

        public Result RemoveIntakeRegistration(int id)
        {
            var foundRegistration = GetIntakeRegistration(id);
            if (foundRegistration == null)
            {
                return new Result(false, "No intake with given id exists");
            }

            foundRegistration.Status = Domain.EntityStatus.Archived.ToString().ToLower(); //Soft-Delete
            _context.IntakeRegistrations.Update(foundRegistration);
            _context.SaveChanges();
            return new Result(true);
        }

        public Result ValidateIntakeRegistration(IntakeRegistration intakeRegistration)
        {
            if (_medicineRepo.GetMedicine(intakeRegistration.IntakeId) == null)
            {
                return new Result(false, "The given intake id does not exist");
            }

            if (intakeRegistration.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                return new Result(false, "The intake date should not be in the past");
            }

            return new Result(true);
        }
    }
}
