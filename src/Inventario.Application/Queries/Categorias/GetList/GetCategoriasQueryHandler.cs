using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Categorias.GetList
{
    internal sealed class GetCategoriasQueryHandler : IRequestHandler<GetCategoriasQuery, PagedResult<CategoriaDto>>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public GetCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
        }
        public async Task<PagedResult<CategoriaDto>> Handle(GetCategoriasQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _categoriaRepository.GetPagedCategoriasAsync(
                request.Page, 
                request.PageSize, 
                request.SearchTerm, 
                cancellationToken);

            var dtos = items.Select(u => new CategoriaDto(
                u.Id,
                u.Codigo,
                u.Descripcion,
                u.Valores,
                u.Ubicacion?.Nombre ?? "Sin Ubicacion",
                u.Ubicacion?.Descripcion ?? "Sin descripcion",
                u.IsActive)).ToList().AsReadOnly();

            return new PagedResult<CategoriaDto>(dtos, totalCount, request.Page, request.PageSize);
        }
    }
}
