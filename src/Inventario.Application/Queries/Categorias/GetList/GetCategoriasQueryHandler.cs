using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Categorias.GetList
{
    internal sealed class GetCategoriasQueryHandler : IRequestHandler<GetCategoriasQuery, IReadOnlyList<CategoriaDto>>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public GetCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
        }
        public async Task<IReadOnlyList<CategoriaDto>> Handle(GetCategoriasQuery request, CancellationToken cancellationToken)
        {
            var categorias = await _categoriaRepository.GetAllAsync(cancellationToken);
            return categorias.Select(u => new CategoriaDto(
                u.Id,
                u.Codigo,
                u.Descripcion,
                u.Valores,
                u.Ubicacion.Nombre,
                u.Ubicacion.Descripcion ?? "Sin descripcion",
                u.IsActive)).ToList().AsReadOnly();
        }
    }
}
