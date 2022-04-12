using Domain.Entities.MedicalData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeders
{
    internal class PatientIntakeSeeder
    {
        public static void SeedPatientIntake(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientIntake>().HasData(
                new PatientIntake() { Id = 1, PatientId = 1, MedicineId = 1, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 2, PatientId = 1, MedicineId = 2, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(11, 59, 59) },
                new PatientIntake() { Id = 3, PatientId = 1, MedicineId = 2, Amount = 1, IntakeStart = new TimeOnly(12, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 4, PatientId = 1, MedicineId = 3, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 5, PatientId = 1, MedicineId = 4, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 6, PatientId = 1, MedicineId = 5, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 7, PatientId = 1, MedicineId = 6, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 8, PatientId = 1, MedicineId = 7, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 9, PatientId = 1, MedicineId = 8, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) },
                new PatientIntake() { Id = 10, PatientId = 1, MedicineId = 9, Amount = 1, IntakeStart = new TimeOnly(0, 0, 0), IntakeEnd = new TimeOnly(23, 59, 59) }
            );
        }
    }
}
