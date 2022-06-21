using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Moq;

namespace dataservice.Tests
{
    public class DatabaseTestBase
    {
        protected readonly Mock<IConfiguration> _mockConfig;

        public DatabaseTestBase()
        {
            _mockConfig = new Mock<IConfiguration>();
        }

        internal MedicineDbContext CreateMedicineTestContext()
        {
            var memoryConnection = new MedicineDbTestConnection();
            var _context = new MedicineDbContext(memoryConnection.GetOptions());
            _context.Database.EnsureCreated();

            return _context;
        }

        internal PatientDbContext CreatePatientTestContext()
        {
            var memoryConnection = new PatientDbTestConnection();
            var _context = new PatientDbContext(memoryConnection.GetOptions());
            _context.Database.EnsureCreated();

            return _context;
        }
    }
}
