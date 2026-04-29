using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(InventarioDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Categoria?> GetByCodeAndUbicacionIdAsync(string code, Guid ubicacionId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Categorias
                .Include(c => c.Ubicacion) // Opcional: solo si necesitas saber si es SOTANO/TALLER
                .FirstOrDefaultAsync(c => c.Codigo == code.ToUpper().Trim() && c.UbicacionId == ubicacionId, cancellationToken);
        }
        public async Task<IReadOnlyList<Categoria>> SearchByTermAsync(string termino, CancellationToken cancellationToken)
        {
            return await DbContext.Categorias
                .AsNoTracking()
                .Include(c => c.Ubicacion)
                .Where(c => c.Descripcion.StartsWith(termino) || c.Codigo.StartsWith(termino)).Take(10)
                .ToListAsync(cancellationToken);
        }

        public override async Task<List<Categoria>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Categorias
                .AsNoTracking()
                .Include(c => c.Ubicacion)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Categoria>> SearchByListCodeAsync(List<string> codes, CancellationToken cancellationToken)
        {
            return await DbContext.Categorias
                .AsNoTracking()
                .Where(s => codes.Contains(s.Codigo.ToUpper()))
                .ToListAsync(cancellationToken);
        }


        public async Task<IReadOnlyList<Categoria>> GetByListCodeAsync(List<string> codeCategoria, CancellationToken cancellationToken)
        {
            if (codeCategoria == null || !codeCategoria.Any())
                return new List<Categoria>();

            var codes = codeCategoria
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.ToUpperInvariant())
                .ToList();

            return await DbContext.Categorias
                .AsNoTracking()
                .Where(s => codes.Contains(s.Codigo.ToUpper()))
                .ToListAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<Categoria> Items, int TotalCount)> GetPagedCategoriasAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = DbContext.Categorias
                .AsNoTracking()
                .Include(c => c.Ubicacion)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(c => 
                    c.Codigo.ToLower().Contains(search) || 
                    c.Descripcion.ToLower().Contains(search) || 
                    (c.Valores != null && c.Valores.ToLower().Contains(search)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}