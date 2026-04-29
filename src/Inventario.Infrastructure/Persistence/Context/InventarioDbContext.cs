using Inventario.Application.Data;
using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventario.Infrastructure.Persistence.Context
{
    public class InventarioDbContext : DbContext, IInventarioDbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public InventarioDbContext(DbContextOptions<InventarioDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        // DbSet's
        public DbSet<Activo> Activos => Set<Activo>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Asignacion> Asignaciones => Set<Asignacion>();
        public DbSet<Mantenimiento> Mantenimientos => Set<Mantenimiento>();
        public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();
        public DbSet<SubCategoria> SubCategorias => Set<SubCategoria>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todas las configuraciones (IEntityTypeConfiguration) del assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventarioDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Configuración global de convenciones para .NET 10.
        /// Esto soluciona el error de PostgreSQL para TODOS los DateTime del proyecto.
        /// </summary>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Define un conversor que asegura que todo DateTime sea UTC al entrar y salir
            configurationBuilder
                .Properties<DateTime>()
                .HaveConversion<DateTimeToUtcConverter>();

            configurationBuilder
                .Properties<DateTime?>()
                .HaveConversion<DateTimeToUtcConverterNullable>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Capturar eventos de las entidades antes de persistir
            var domainEvents = ChangeTracker.Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e => e.GetDomainEvents())
                .ToList(); // Materializar para no perderlos tras el Save

            // 2. Persistir cambios en la base de datos
            var result = await base.SaveChangesAsync(cancellationToken);

            // 3. Publicar eventos (Post-Save)
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }

    // --- CONVERSORES AUXILIARES ---

    /// <summary>
    /// Fuerza el Kind de DateTime a Utc para compatibilidad estricta con PostgreSQL (Npgsql)
    /// </summary>
    public class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeToUtcConverter()
            : base(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        { }
    }

    public class DateTimeToUtcConverterNullable : ValueConverter<DateTime?, DateTime?>
    {
        public DateTimeToUtcConverterNullable()
            : base(
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
        { }
    }
}