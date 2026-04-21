using Inventario.Application.Data;
using Inventario.Domain.Primitives;
using Inventario.Infrastructure.Persistence.Context;
using Inventario.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventario.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration)
                    .AddRepositories()
                    .AddServices();
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventarioDbContext>(options  => options.UseNpgsql(configuration.GetConnectionString("InventarioDb")));
            services.AddScoped<IInventarioDbContext>(sp => sp.GetRequiredService<InventarioDbContext>());
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<InventarioDbContext>());

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = typeof(Repository<>).Assembly;

            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"));

            foreach (var type in repositoryTypes)
            {
                // Buscamos la interfaz que corresponde (ej: IUsuarioRepository para UsuarioRepository)
                var interfaceType = type.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{type.Name}");

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<Inventario.Application.Interfaces.Services.IExcelExportService, Inventario.Infrastructure.Services.ExcelExportService>();
            return services;
        }
    }
}
