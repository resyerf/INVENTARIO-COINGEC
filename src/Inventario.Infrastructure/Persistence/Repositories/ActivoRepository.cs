using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class ActivoRepository : Repository<Activo>, IActivoRepository
    {
        public ActivoRepository(InventarioDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Activo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                .Include(a => a.SubCategoria)
                    .ThenInclude(s => s.Categoria) // Navegación jerárquica
                .Include(a => a.Usuario)
                .Include(a => a.Ubicacion)
                .Include(a => a.Asignaciones)
                .Include(a => a.Mantenimientos)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Activo?> GetBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                .FirstOrDefaultAsync(a => a.Serie == serialNumber, cancellationToken);
        }

        // Corregido: Ahora filtramos por la Categoría que está dentro de la SubCategoría
        public async Task<List<Activo>> GetByCategoriaAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                .Where(a => a.SubCategoria.CategoriaId == categoryId)
                .Include(a => a.SubCategoria)
                .Include(a => a.Usuario)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Activo>> GetActivosAsignadosAUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.Ubicacion)
                .Include(a => a.SubCategoria)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Activo>> GetByUbicacionAsync(Guid ubicacionId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                .Where(a => a.UbicacionId == ubicacionId)
                .Include(a => a.Usuario)
                .Include(a => a.SubCategoria)
                    .ThenInclude(s => s.Categoria)
                .ToListAsync(cancellationToken);
        }
    }
}