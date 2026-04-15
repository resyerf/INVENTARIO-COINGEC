using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class SubCategoryRepository : Repository<SubCategoria>, ISubCategoryRepository
    {
        public SubCategoryRepository(InventarioDbContext dbContext) : base(dbContext)
        {

        }
    }
}
