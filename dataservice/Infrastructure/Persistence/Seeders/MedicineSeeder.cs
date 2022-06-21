using Domain.Entities.MedicalData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeders
{
    internal static class MedicineSeeder
    {
        public static void SeedMedicine(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine() { Id = 1, Name = "Rosuvastatine", Indication = "Hypercholesterolemie", Dose = 10, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://va-cdn.azureedge.net/images/Rosuvastatine.jpg" },
                new Medicine() { Id = 2, Name = "Hydrochloorthiazide", Indication = "Hypertensei", Dose = 12.5, DoseUnit = "mg", Type = "Tablet", Shape = "Rond", Color = "Rood", ImageURL = "https://va-cdn.azureedge.net/images/Hydrochloorthiazide.jpg" },
                new Medicine() { Id = 3, Name = "Metformine", Indication = "Diabetes Mellitus type 2", Dose = 500, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://va-cdn.azureedge.net/images/Metformine.jpg" },
                new Medicine() { Id = 4, Name = "Pantroprazol msr", Indication = "Maagbeschermer", Dose = 80, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Blauw", ImageURL = "https://va-cdn.azureedge.net/images/Pantoprazol-msr.jpg" },
                new Medicine() { Id = 5, Name = "Nitrofurantione", Indication = "Antibiotica (Urineweginfectie)", Dose = 50, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://va-cdn.azureedge.net/images/Nitrofurantione.jpg" },
                new Medicine() { Id = 6, Name = "Temazepam", Indication = "Somberheid", Dose = 10, DoseUnit = "mg", Type = "Tablet", Shape = "Rond", Color = "Groen", ImageURL = "https://va-cdn.azureedge.net/images/Temazepam.jpg" },
                new Medicine() { Id = 7, Name = "Furosemide", Indication = "Hartfalen", Dose = 40, DoseUnit = "mg", Type = "Tablet", Shape = "Hexagonaal", Color = "Wit", ImageURL = "https://va-cdn.azureedge.net/images/Furosemide.jpg" },
                new Medicine() { Id = 8, Name = "Finasteride", Indication = "Nycturie", Dose = 5, DoseUnit = "mg", Type = "Capsule", Shape = "Rond", Color = "Zwart", ImageURL = "https://va-cdn.azureedge.net/images/Finasteride.jpg" },
                new Medicine() { Id = 9, Name = "Oxazepam", Indication = "Slaapproblemen", Dose = 10, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://va-cdn.azureedge.net/images/Oxazepam.jpg" }
                );
        }
    }
}
