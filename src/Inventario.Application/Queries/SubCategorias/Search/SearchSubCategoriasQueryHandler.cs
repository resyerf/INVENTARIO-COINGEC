using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.SubCategorias.Search
{
    internal sealed class SearchSubCategoriasQueryHandler : IRequestHandler<SearchSubCategoriasQuery, Result<IReadOnlyList<SubCategoriaDto>>>
    {
        private readonly ISubCategoryRepository _repository;

        public SearchSubCategoriasQueryHandler(ISubCategoryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<Result<IReadOnlyList<SubCategoriaDto>>> Handle(SearchSubCategoriasQuery request, CancellationToken cancellationToken)
        {
            // Si no hay término, podrías devolver todas las de esa categoría o lista vacía
            var subCategorias = await _repository.GetByCategoriaAndTermAsync(
                request.Termino,
                cancellationToken);

            return Result<IReadOnlyList<SubCategoriaDto>>.Success(subCategorias.Select(s => new SubCategoriaDto(
                s.Id,
                s.Nombre,
                s.Categoria.Codigo ?? "N/A",
                s.Categoria.Descripcion ?? "N/A",
                s.IsActive
            )).ToList().AsReadOnly());
        }
    }
}
