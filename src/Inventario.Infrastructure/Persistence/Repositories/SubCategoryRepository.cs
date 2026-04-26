using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class SubCategoryRepository : Repository<SubCategoria>, ISubCategoryRepository
    {
        public SubCategoryRepository(InventarioDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IReadOnlyList<SubCategoria>> GetAllWithIncludesAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.SubCategorias
                .AsNoTracking()
                .Include(s => s.Categoria)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SubCategoria>> GetByCategoriaAndTermAsync(string termino, CancellationToken cancellationToken)
        {
            return await DbContext.SubCategorias
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Where(c => c.Nombre.ToLower().StartsWith(termino.ToLower()))
                .Take(10)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SubCategoria>> GetByCategoriaListCodeAsync(List<string> codeCategoria, CancellationToken cancellationToken)
        {
            if (codeCategoria == null || !codeCategoria.Any())
                return new List<SubCategoria>();

            // Normalizar (igual que haces con ToUpperInvariant en otros lados)
            var codes = codeCategoria
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.ToUpperInvariant())
                .ToList();

            return await DbContext.SubCategorias
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Where(s => codes.Contains(s.Categoria.Codigo.ToUpper()))
                .ToListAsync(cancellationToken);
        }
    }
}
