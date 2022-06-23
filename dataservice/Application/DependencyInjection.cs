using Application.Repositories;
using Application.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            Infrastructure.DependencyInjection.AddInfrastructure(services, configuration);

            services.AddScoped<IColorRepo, ColorRepo>();
            services.AddScoped<IShapeRepo, ShapeRepo>();
            services.AddScoped<ITypeRepo, TypeRepo>();
            services.AddScoped<IDoseUnitRepo, DoseUnitRepo>();
            services.AddScoped<IMedicineRepo, MedicineRepo>();
            services.AddScoped<IPatientRepo, PatientRepo>();
            services.AddScoped<IPatientIntakeRepo, PatientIntakeRepo>();
            services.AddScoped<IIntakeRegistrationRepo, IntakeRegistrationRepo>();
            services.AddScoped<IPatientDeviceRepo, PatientDeviceRepo>();

            services.AddScoped<ICareWorkerFunctionRepo, CareWorkerFunctionRepo>();
            services.AddScoped<ICareWorkerRepo, CareWorkerRepo>();

            return services;
        }
    }
}
