using Inventario.Application.Data;
using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Context
{
    public class InventarioDbContext : DbContext, IInventarioDbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;
        public InventarioDbContext(DbContextOptions<InventarioDbContext> options, IPublisher publisher) : base(options)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        public DbSet<Activo> Activos => Set<Activo>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Asignacion> Asignaciones => Set<Asignacion>();
        public DbSet<Mantenimiento> Mantenimientos => Set<Mantenimiento>();
        public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();

        public DbSet<SubCategoria> SubCategorias => Set<SubCategoria>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventarioDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker.Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e => e.GetDomainEvents());

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
