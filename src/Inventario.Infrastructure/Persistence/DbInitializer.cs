using Inventario.Application.Common.Interfaces;
using Inventario.Domain.Entities;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventario.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAuthAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<InventarioDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            // Ejecutar migraciones si es necesario (opcional según flujo del proyecto)
            // await context.Database.MigrateAsync();

            if (!await context.AuthUsers.AnyAsync())
            {
                var adminUser = AuthUser.Create(
                    "admin",
                    hasher.Hash("admin"),
                    "Admin"
                );

                context.AuthUsers.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
