using Domain.Entities.PatientData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class PatientDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientDeviceIdentifier> PatientDeviceIdentifiers { get; set; }
        public DbSet<EmergencyNotice> EmergencyNotices { get; set; }

        public DbSet<PatientLocation> PatientLocations { get; set; }

        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.EmergencyNotices)
                .WithOne(en => en.Patient);

            modelBuilder.Entity<PatientLocation>().HasData(
                new PatientLocation() { Id = 1, RoomId = "1"},
                new PatientLocation() { Id = 2, RoomId = "2"}
            );
            modelBuilder.Entity<Patient>().HasData(
                new Patient() { Id = 1, FirstName = "Test", LastName = "Tester", BirthDate = new DateTime(1993, 10, 20), Email = "testtester@test.com", PhoneNumber = "0687654321", PostalCode = "5223 DE", HomeNumber = "215", LocationId = 1 },
                new Patient() { Id = 2, FirstName = "Teste", LastName = "Anderson", BirthDate = new DateTime(1998, 10, 20), Email = "testtesteranderson@test.com", PhoneNumber = "0612346789", PostalCode = "1234 DE", HomeNumber = "123", LocationId = 2 }
            );
        }
    }
}
