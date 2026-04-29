using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using MediatR;

using Inventario.Application.Common.Models;

namespace Inventario.Application.Queries.Categorias.GetList
{
    public record GetCategoriasQuery(
        int Page = 1,
        int PageSize = 10,
        string? SearchTerm = null
    ) : IRequest<Result<PagedResult<CategoriaDto>>>;
}
