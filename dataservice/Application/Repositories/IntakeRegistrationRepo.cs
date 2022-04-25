using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class IntakeRegistrationRepo : IIntakeRegistrationRepo
    {
        private MedicineDbContext _context;

        public IntakeRegistrationRepo(MedicineDbContext context)
        {
            this._context = context;
        }

        public Result AddIntakeRegistration(DateOnly date, int patientIntakeId)
        {
            throw new NotImplementedException();
        }

        public IntakeRegistration? GetIntakeRegistration(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IntakeRegistration> GetIntakeRegistrationForDate()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
