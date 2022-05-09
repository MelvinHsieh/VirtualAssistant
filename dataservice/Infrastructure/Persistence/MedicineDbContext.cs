using Domain.Entities.MedicalData;
using Infrastructure.Persistence.Seeders;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class MedicineDbContext : DbContext
    {
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<MedicineColor> Medicine_Colors { get; set; }
        public DbSet<MedicineShape> Medicine_Shapes { get; set; }
        public DbSet<MedicineType> Medicine_Types { get; set; }
        public DbSet<DoseUnit> DoseUnits { get; set; }
        public DbSet<PatientIntake> PatientIntakes { get; set; }
        public DbSet<IntakeRegistration> IntakeRegistrations { get; set; }
        public DbSet<CareWorker> CareWorkers { get; set; }
        public DbSet<CareWorkerFunction> CareWorkerFunctions { get; set; }

        public MedicineDbContext(DbContextOptions<MedicineDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Allow the use of timeonly object
            modelBuilder.Entity<PatientIntake>(builder =>
            {
                builder.Property(x => x.IntakeStart)
                    .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();

                builder.Property(x => x.IntakeEnd)
                    .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
            });

            //Lay the lookup table keys
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

            modelBuilder.Entity<CareWorker>()
                .HasOne<CareWorkerFunction>()
                .WithMany()
                .HasForeignKey(m => m.Function);

            //Seed Data
            MedicineIdentifierSeeder.SeedMedicineIdentifiers(modelBuilder);
            MedicineSeeder.SeedMedicine(modelBuilder);
            PatientIntakeSeeder.SeedPatientIntake(modelBuilder);
            CareWorkerFunctionSeeder.SeedCareWorkerFunction(modelBuilder);
            CareWorkerSeeder.SeedCareWorker(modelBuilder);
        }
    }
}
