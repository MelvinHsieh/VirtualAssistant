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

            services.AddDbContext<MedicineDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("MedicineConnection"),
                    b => b.MigrationsAssembly(typeof(MedicineDbContext).Assembly.FullName)));

            services.AddDbContext<PatientDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("PatientConnection"),
                    b => b.MigrationsAssembly(typeof(PatientDbContext).Assembly.FullName)));


            return services;
        }
    }
}
