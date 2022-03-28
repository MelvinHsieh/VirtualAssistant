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

            services.AddSingleton<IColorRepo, ColorRepo>();
            services.AddSingleton<IShapeRepo, ShapeRepo>();
            services.AddSingleton<ITypeRepo, TypeRepo>();
            services.AddSingleton<IDoseUnitRepo, DoseUnitRepo>();
            services.AddSingleton<IMedicineRepo, MedicineRepo>();

            return services;
        }
    }
}
