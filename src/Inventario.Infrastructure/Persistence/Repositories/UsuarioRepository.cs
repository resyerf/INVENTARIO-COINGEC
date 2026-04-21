using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(InventarioDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Usuario>> GetByAreaAsync(string area, CancellationToken cancellationToken = default)
    {
        // Pura persistencia: Si el área viene normalizada de arriba, aquí solo filtramos
        return await DbContext.Usuarios
            .AsNoTracking()
            .Where(u => u.Area == area)
            .ToListAsync(cancellationToken);
    }

    public async Task<Usuario?> GetByFullNameAsync(string nombreCompleto, CancellationToken cancellationToken = default)
    {
        // Buscamos el objeto tal cual nos pasan el parámetro
        return await DbContext.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreCompleto == nombreCompleto, cancellationToken);
    }

    public async Task<Usuario?> GetByDocumentNbrAsync(string documentNbr, CancellationToken cancellation = default)
    {
        // B-Tree Index optimizado
        return await DbContext.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DocumentoIdentidad == documentNbr, cancellation);
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellation = default)
    {
        // B-Tree Index optimizado
        return await DbContext.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellation);
    }

    public async Task<IReadOnlyList<Usuario>> GetBySearchTermAsync(string termino, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Usuarios.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(termino))
        {
            var terminoLower = termino.ToLower();
            query = query.Where(u => u.NombreCompleto.ToLower().Contains(terminoLower));
        }

        return await query.Take(10).ToListAsync(cancellationToken);
    }
}