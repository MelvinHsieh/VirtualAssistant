using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }
    }
}
