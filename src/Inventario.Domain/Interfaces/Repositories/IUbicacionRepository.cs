using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories;

public interface IUbicacionRepository : IRepository<Ubicacion>
{
    // Método específico para validar por nombre (SOTANO, TALLER)
    Task<Ubicacion?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}