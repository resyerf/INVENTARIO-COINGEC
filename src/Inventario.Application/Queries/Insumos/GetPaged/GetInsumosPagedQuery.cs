using Inventario.Application.Common.Models;
using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Insumos.GetPaged
{
    public record GetInsumosPagedQuery(
        int Page = 1,
        int PageSize = 10,
        string? SearchTerm = null)
    : IRequest<Result<PagedResult<InsumoDto>>>;
}
