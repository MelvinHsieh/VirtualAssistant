using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var medicineConnection = configuration.GetConnectionString("MedicineConnection");
            var patientConnection = configuration.GetConnectionString("PatientConnection");
            var userID = configuration.GetConnectionString("DbUserID");
            var userPass = configuration.GetConnectionString("DbUserPassword");

            services.AddDbContext<MedicineDbContext>(options =>
                options.UseSqlServer(
                    $"{medicineConnection}User Id=${userID};Password=${userPass};",
                    b => b.MigrationsAssembly(typeof(MedicineDbContext).Assembly.FullName)));

            services.AddDbContext<PatientDbContext>(options =>
                options.UseSqlServer(
                    $"{patientConnection}User Id=${userID};Password=${userPass};",
                    b => b.MigrationsAssembly(typeof(PatientDbContext).Assembly.FullName)));


            return services;
        }
    }
}
