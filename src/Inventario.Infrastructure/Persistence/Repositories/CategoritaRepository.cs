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

        public async Task<Categoria?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await DbContext.Categorias
                .Include(c => c.Ubicacion) // Opcional: solo si necesitas saber si es SOTANO/TALLER
                .FirstOrDefaultAsync(c => c.Codigo == code.ToUpper().Trim(), cancellationToken);
        }
    }
}