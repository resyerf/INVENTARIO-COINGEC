using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        // Buscar por el código de categoría (Ej: HERM)
        Task<Categoria?> GetByCodeAndUbicacionIdAsync(string code, Guid ubicacionId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Categoria>> SearchByTermAsync(string termino, CancellationToken cancellationToken);
        Task<IReadOnlyList<Categoria>> SearchByListCodeAsync(List<string> codes, CancellationToken cancellationToken);
    }
}
