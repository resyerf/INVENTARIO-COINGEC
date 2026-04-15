using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Inventario.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ApplicationAssemblyReference)));

            services.AddValidatorsFromAssemblyContaining(typeof(ApplicationAssemblyReference));
            return services;
        }
    }
}
