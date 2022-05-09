using Domain.Entities.MedicalData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeders
{
    internal static class MedicineIdentifierSeeder
    {
        internal static void SeedMedicineIdentifiers(ModelBuilder modelBuilder)
        {
            SeedShapes(modelBuilder);
            SeedColors(modelBuilder);
            SeedTypes(modelBuilder);
            SeedDoseUnits(modelBuilder);
        }

        private static void SeedShapes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineShape>().HasData(
                new MedicineShape("Vierkant"),
                new MedicineShape("Hexagonaal"),
                new MedicineShape("Rond")
            );
        }

        private static void SeedColors(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineColor>().HasData(
                new MedicineColor("Rood"),
                new MedicineColor("Wit"),
                new MedicineColor("Blauw"),
                new MedicineColor("Geel"),
                new MedicineColor("Groen"),
                new MedicineColor("Zwart")
            );
        }

        private static void SeedTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineType>().HasData(
                new MedicineType("Pil"),
                new MedicineType("Tablet"),
                new MedicineType("Capsule"),
                new MedicineType("Spuit")
            );
        }

        private static void SeedDoseUnits(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoseUnit>().HasData(
                new DoseUnit("mg"),
                new DoseUnit("µg")
            );
        }
    }
}
