using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Activos.GetList
{
    public sealed record GetActivosQuery(
        int Page,
        int PageSize,
        string? SearchTerm,
        string? Condicion,
        bool? IsActive,
        string? Categoria,
        string? Custodio
    ) : IRequest<PagedResult<ActivoDto>>;
}
