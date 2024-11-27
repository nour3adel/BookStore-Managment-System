using BookStore.Infrastructure.Common;
using BookStore.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastucture
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<UnitOFWork>();
            //services.AddScoped<IAttendanceRepository, AttendanceRepostory>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
