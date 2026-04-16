using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface ISubCategoryRepository : IRepository<SubCategoria>
    {
        Task<IReadOnlyList<SubCategoria>> GetAllWithIncludesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SubCategoria>> GetByCategoriaAndTermAsync(string termino, CancellationToken cancellationToken);
    }
}
