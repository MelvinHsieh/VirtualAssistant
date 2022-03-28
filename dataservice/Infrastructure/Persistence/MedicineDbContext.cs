using Domain.Entities.MedicalData;
using Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal class MedicineDbContext : DbContext
    {
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<MedicineColor> Medicine_Colors { get; set; }
        public DbSet<MedicineShape> Medicine_Shapes { get; set; }
        public DbSet<MedicineType> Medicine_Types { get; set; }
        public DbSet<DoseUnit> DoseUnits { get; set; }

        public MedicineDbContext(DbContextOptions<MedicineDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicine>()
                .HasOne<MedicineColor>()
                .WithMany()
                .HasForeignKey(m => m.Color);

            modelBuilder.Entity<Medicine>()
                .HasOne<MedicineShape>()
                .WithMany()
                .HasForeignKey(m => m.Shape);

            modelBuilder.Entity<Medicine>()
                .HasOne<MedicineType>()
                .WithMany()
                .HasForeignKey(m => m.Type);

            modelBuilder.Entity<Medicine>()
                .HasOne<DoseUnit>()
                .WithMany()
                .HasForeignKey(m => m.DoseUnit);

            MedicineIdentifierSeeder.SeedMedicineIdentifiers(modelBuilder);
            MedicineSeeder.SeedMedicine(modelBuilder);
        }
    }
}
