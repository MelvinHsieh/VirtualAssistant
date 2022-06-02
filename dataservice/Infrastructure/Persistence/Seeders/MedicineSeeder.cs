using Domain.Entities.MedicalData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeders
{
    internal static class MedicineSeeder
    {
        public static void SeedMedicine(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine() { Id = 1, Name = "Rosuvastatine", Indication = "Hypercholesterolemie", Dose = 10, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 2, Name = "Hydrochloorthiazide", Indication = "Hypertensei", Dose = 12.5, DoseUnit = "mg", Type = "Tablet", Shape = "Rond", Color = "Rood", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 3, Name = "Metformine", Indication = "Diabetes Mellitus type 2", Dose = 500, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 4, Name = "Pantroprazol msr", Indication = "Maagbeschermer", Dose = 80, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Blauw", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 5, Name = "Nitrofurantione", Indication = "Antibiotica (Urineweginfectie)", Dose = 50, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 6, Name = "Temazepam", Indication = "Somberheid", Dose = 10, DoseUnit = "mg", Type = "Tablet", Shape = "Rond", Color = "Groen", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 7, Name = "Furosemide", Indication = "Hartfalen", Dose = 40, DoseUnit = "mg", Type = "Tablet", Shape = "Hexagonaal", Color = "Wit", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 8, Name = "Finasteride", Indication = "Nycturie", Dose = 5, DoseUnit = "mg", Type = "Capsule", Shape = "Rond", Color = "Zwart", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" },
                new Medicine() { Id = 9, Name = "Oxazepam", Indication = "Slaapproblemen", Dose = 10, DoseUnit = "mg", Type = "Tablet", Shape = "Vierkant", Color = "Wit", ImageURL = "https://i.imgur.com/xOhZ29x.jpg" }
                );
        }
    }
}
