using Inventario.Application.Common.Interfaces;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Repositories;
using Inventario.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Inventario.Infrastructure.Extensions
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthUserRepository, AuthUserRepository>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenProvider, TokenProvider>();
            
            return services;
        }
    }
}
