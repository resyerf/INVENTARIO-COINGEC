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
                .Include(a => a.Categoria)
                    //.ThenInclude(s => s.Categoria) // Navegación jerárquica
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
                .Where(a => a.CategoriaId == categoryId)
                //.Include(a => a.SubCategoria)
                .Include(a => a.Usuario)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Activo>> GetActivosAsignadosAUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.Ubicacion)
                //.Include(a => a.SubCategoria)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Activo>> GetByUbicacionAsync(Guid ubicacionId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                .Where(a => a.UbicacionId == ubicacionId)
                .Include(a => a.Usuario)
                .Include(a => a.Categoria)
                    //.ThenInclude(s => s.Categoria)
                .ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<Activo>> GetAllForReportAsync(CancellationToken ct)
        {
            return await DbContext.Activos
                .AsNoTracking()
                .Include(a => a.Categoria)
                    //.ThenInclude(s => s.Categoria)
                .Include(a => a.Usuario)
                .Include(a => a.Ubicacion)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Activo>> GetBySearchTermAsync(string termino, CancellationToken cancellationToken = default)
        {
            var query = DbContext.Activos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(termino))
            {
                var terminoLower = termino.ToLower();
                query = query.Where(u => u.NombreEquipo.ToLower().Contains(terminoLower));
            }

            return await query.Take(10).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Activo>> GetExistingCodesAsync(List<string> codigosEquipo, CancellationToken cancellationToken = default)
        {
            return await DbContext.Activos
                        .AsNoTracking()
                        .Where(u => u.CodigoEquipo != null && codigosEquipo.Contains(u.CodigoEquipo))
                        .ToListAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<Activo> Items, int TotalCount)> GetPagedActivosAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm, 
            string? condicion, 
            bool? isActive, 
            string? categoria, 
            string? custodio, 
            CancellationToken cancellationToken = default)
        {
            var query = DbContext.Activos
                .AsNoTracking()
                .Include(a => a.Categoria)
                .Include(a => a.Usuario)
                .Include(a => a.Ubicacion)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(a => 
                    a.NombreEquipo.ToLower().Contains(search) || 
                    (a.Observaciones != null && a.Observaciones.ToLower().Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(condicion))
            {
                var cond = condicion.ToLower();
                query = query.Where(a => a.Estado != null && a.Estado.ToLower().Contains(cond));
            }

            if (isActive.HasValue)
            {
                query = query.Where(a => a.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                var cat = categoria.ToLower();
                query = query.Where(a => a.Categoria != null && (
                    a.Categoria.Codigo.ToLower().Contains(cat) ||
                    a.Categoria.Descripcion.ToLower().Contains(cat) ||
                    (a.Categoria.Valores != null && a.Categoria.Valores.ToLower().Contains(cat))
                ));
            }

            if (!string.IsNullOrWhiteSpace(custodio))
            {
                var cust = custodio.ToLower();
                query = query.Where(a => a.Usuario != null && (
                    a.Usuario.NombreCompleto.ToLower().Contains(cust) ||
                    a.Usuario.Email.ToLower().Contains(cust)
                ));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(a => a.Id) // Basic ordering, might change later
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}