using Inventario.Application.DTOs;
using MediatR;
using Inventario.Application.Common.Models;
using Inventario.Application.Common.Pagination;

namespace Inventario.Application.Queries.Solicitudes.GetPaged
{
    public record GetSolicitudesPagedQuery(int PageNumber, int PageSize, string? SearchTerm) 
        : IRequest<Result<PagedResult<SolicitudInsumoDto>>>;
}
