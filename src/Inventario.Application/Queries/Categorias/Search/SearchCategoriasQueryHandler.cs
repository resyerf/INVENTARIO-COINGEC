using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Categorias.Search
{
    internal sealed class SearchCategoriasQueryHandler : IRequestHandler<SearchCategoriasQuery, IReadOnlyList<CategoriaDto>>
    {
        private readonly ICategoriaRepository _repository;

        public SearchCategoriasQueryHandler(ICategoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IReadOnlyList<CategoriaDto>> Handle(SearchCategoriasQuery request, CancellationToken cancellationToken)
        {
            // Si el término es nulo o vacío, devolvemos lista vacía o podrías devolver las top 10
            if (string.IsNullOrWhiteSpace(request.Termino))
                return new List<CategoriaDto>().AsReadOnly();

            // Buscamos en el repositorio filtrando por el inicio del nombre
            var categorias = await _repository.SearchByTermAsync(request.Termino, cancellationToken);

            return categorias.Select(c => new CategoriaDto(
                c.Id,
                c.Codigo,
                c.Descripcion,
                c.Ubicacion.Nombre,
                c.Ubicacion.Descripcion ?? "Sin descripcion"
            )).ToList().AsReadOnly();
        }
    }
}
