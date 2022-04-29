using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.MedicalData
{
    public class IntakeRegistration
    {
        public int Id { get; set; }
        public DateTime TakenOn { get; set; }
        public int PatientIntakeId { get; set; }

        public virtual PatientIntake PatientIntake { get; set; }
    }
}
