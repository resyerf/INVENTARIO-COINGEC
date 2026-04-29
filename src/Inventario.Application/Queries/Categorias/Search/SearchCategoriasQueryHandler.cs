using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;
using Inventario.Application.Common.Models;

namespace Inventario.Application.Queries.Categorias.Search
{
    internal sealed class SearchCategoriasQueryHandler : IRequestHandler<SearchCategoriasQuery, Result<IReadOnlyList<CategoriaDto>>>
    {
        private readonly ICategoriaRepository _repository;

        public SearchCategoriasQueryHandler(ICategoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<IReadOnlyList<CategoriaDto>>> Handle(SearchCategoriasQuery request, CancellationToken cancellationToken)
        {
            // Si el término es nulo o vacío, devolvemos lista vacía o podrías devolver las top 10
            if (string.IsNullOrWhiteSpace(request.Termino))
                return Result<IReadOnlyList<CategoriaDto>>.Success(new List<CategoriaDto>().AsReadOnly());

            // Buscamos en el repositorio filtrando por el inicio del nombre
            var categorias = await _repository.SearchByTermAsync(request.Termino.ToUpper(), cancellationToken);

            return Result<IReadOnlyList<CategoriaDto>>.Success(categorias.Select(c => new CategoriaDto(
                c.Id,
                c.Codigo,
                c.Descripcion,
                c.Valores,
                c.Ubicacion?.Nombre ?? "-",
                c.Ubicacion?.Descripcion ?? "Sin descripcion",
                c.IsActive
            )).ToList().AsReadOnly());
        }
    }
}
