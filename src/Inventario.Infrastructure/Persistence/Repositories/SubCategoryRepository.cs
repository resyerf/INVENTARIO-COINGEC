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
    }
}
