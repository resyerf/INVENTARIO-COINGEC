using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories;

public interface IActivoRepository : IRepository<Activo>
{

    // Búsqueda por número de serie (Para evitar duplicados de hardware real)
    Task<Activo?> GetBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default);

    // Obtener activos filtrados por categoría (Útil para reportes de Sótano, Taller, etc.)
    Task<List<Activo>> GetByCategoriaAsync(Guid categoriaId, CancellationToken cancellationToken = default);

    // Obtener todos los activos que un usuario tiene actualmente en su poder
    Task<List<Activo>> GetActivosAsignadosAUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Activo>> GetAllForReportAsync(CancellationToken ct);
}