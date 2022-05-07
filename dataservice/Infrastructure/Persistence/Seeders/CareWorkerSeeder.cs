using Domain.Entities.MedicalData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeders
{
    internal static class CareWorkerSeeder
    {
        public static void SeedCareWorker(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CareWorker>().HasData(
                new CareWorker() { Id = 1, FirstName = "Petra", LastName = "Janssen", Function = "Verpleger" },
                new CareWorker() { Id = 2, FirstName = "Henny", LastName = "Heeren", Function = "Verpleger" },
                new CareWorker() { Id = 3, FirstName = "Peter", LastName = "Peters", Function = "Dokter" },
                new CareWorker() { Id = 4, FirstName = "Frida", LastName = "Leuken", Function = "Dokter" }
                );
        }
    }
}
