using Application.Common.Models;
using Domain.Entities.MedicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Interfaces
{
    public interface IIntakeRegistrationRepo
    {
        public Result AddIntakeRegistration(DateOnly date, int patientIntakeId);

        public IntakeRegistration? GetIntakeRegistration(int id);

        public IEnumerable<IntakeRegistration> GetIntakeRegistrationForDate(); //For a patient

        public Result RemoveIntakeRegistration(int id);

        public Result ValidateIntakeRegistration(IntakeRegistration intakeRegistration);
    }
}
