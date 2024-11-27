using BookStore.Application.Features;
using BookStore.Application.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application
{
    public static class ModuleApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<IBooksServices, BooksServices>();
            services.AddScoped<ICustomerServices, CustomerServices>();
            services.AddScoped<IOrderServices, OrderServices>();

            return services;
        }
    }
}
