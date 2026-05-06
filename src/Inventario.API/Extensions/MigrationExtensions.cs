using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Inventario.Infrastructure.Persistence;

namespace Inventario.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventarioDbContext>();
            dbContext.Database.Migrate();
            
            // Seed Auth
            DbInitializer.SeedAuthAsync(app.Services).Wait();
        }   
    }
}
