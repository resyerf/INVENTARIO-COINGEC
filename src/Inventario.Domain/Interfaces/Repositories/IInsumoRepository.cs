using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface IInsumoRepository : IRepository<Insumo>
    {
        Task<(IReadOnlyList<Insumo> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}
