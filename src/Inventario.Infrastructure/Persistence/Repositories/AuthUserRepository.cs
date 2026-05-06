using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class AuthUserRepository : Repository<AuthUser>, IAuthUserRepository
    {
        public AuthUserRepository(InventarioDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<AuthUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var lowerUsername = username.ToLowerInvariant();
            return await DbContext.Set<AuthUser>()
                .FirstOrDefaultAsync(u => u.Username == lowerUsername, cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<AuthUser>().AnyAsync(cancellationToken);
        }
    }
}
