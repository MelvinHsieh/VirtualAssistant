using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class IntakeRegistrationRepo : IIntakeRegistrationRepo
    {
        private MedicineDbContext _context;


        public IntakeRegistrationRepo(MedicineDbContext context)
        {
            this._context = context;
        }

        public Result AddIntakeRegistration(DateTime date, int patientIntakeId)
        {
            IntakeRegistration registration = new IntakeRegistration()
            {
                TakenOn = date,
                PatientIntakeId = patientIntakeId
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
            return _context.IntakeRegistrations
                .Where(x => x.Id == id)
                .Include(x => x.PatientIntake)
                .Include(x => x.PatientIntake.Medicine)
                .FirstOrDefault();
        }

        public List<IntakeRegistration> GetIntakeRegistrationForDate(int patientId, DateOnly date)
        {
            return _context.IntakeRegistrations
                .Include(x => x.PatientIntake)
                .Include(x => x.PatientIntake.Medicine)
                .Where(x => x.PatientIntake.PatientId == patientId)
                .Where(x => x.TakenOn.Date == date.ToDateTime(TimeOnly.MinValue).Date)
                .Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower())
                .ToList();
        }

        public List<IntakeRegistration> GetIntakeRegistrationForPatient(int patientId)
        {
            return _context.IntakeRegistrations
                .Include(x => x.PatientIntake)
                .Include(x => x.PatientIntake.Medicine)
                .Where(x => x.PatientIntake.PatientId == patientId)
                .Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower())
                .ToList();

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

            if (intakeRegistration.TakenOn < DateTime.Now.Date)
            {
                return new Result(false, "The intake date should not be in the past");
            }

            return new Result(true);
        }
    }
}
