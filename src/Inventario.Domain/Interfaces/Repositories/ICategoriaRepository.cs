using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        // Buscar por el código de categoría (Ej: HERM)
        Task<Categoria?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}
