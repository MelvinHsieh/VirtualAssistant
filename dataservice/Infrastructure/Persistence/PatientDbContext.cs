using Domain.Entities.PatientData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class PatientDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().HasData(
                new Patient() { Id = 1, FirstName = "Test", LastName = "Tester", BirthDate = new DateTime(1993, 10, 20), Email = "testtester@test.com", PhoneNumber = "0687654321", PostalCode = "5223 DE", HomeNumber = "215" }
            );
        }
    }
}
