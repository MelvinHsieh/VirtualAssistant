using Domain.Entities.MedicalData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeders
{
    internal static class CareWorkerFunctionSeeder
    {
        public static void SeedCareWorkerFunction(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CareWorkerFunction>().HasData(
                new CareWorkerFunction() { Name = "Verpleger"},
                new CareWorkerFunction() { Name = "Dokter" }
            );
        }
    }
}
