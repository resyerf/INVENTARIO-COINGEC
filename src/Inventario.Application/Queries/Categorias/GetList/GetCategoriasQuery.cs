using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Categorias.GetList
{
    public record GetCategoriasQuery(
        int Page = 1,
        int PageSize = 10,
        string? SearchTerm = null
    ) : IRequest<PagedResult<CategoriaDto>>;
}
