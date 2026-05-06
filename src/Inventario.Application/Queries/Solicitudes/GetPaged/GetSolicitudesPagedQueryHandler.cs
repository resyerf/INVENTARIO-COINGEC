using Inventario.Application.Common.Models;
using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Solicitudes.GetPaged
{
    internal sealed class GetSolicitudesPagedQueryHandler : IRequestHandler<GetSolicitudesPagedQuery, Result<PagedResult<SolicitudInsumoDto>>>
    {
        private readonly ISolicitudInsumoRepository _repository;

        public GetSolicitudesPagedQueryHandler(ISolicitudInsumoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResult<SolicitudInsumoDto>>> Handle(GetSolicitudesPagedQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(request.PageNumber, request.PageSize, request.SearchTerm, cancellationToken);

            var dtos = items.Select(s => new SolicitudInsumoDto(
                s.Id,
                s.InsumoId,
                s.Insumo?.Nombre ?? "DESCONOCIDO",
                s.Cantidad,
                s.UsuarioId,
                s.Usuario?.NombreCompleto ?? "DESCONOCIDO",
                s.FechaSolicitud,
                s.Estado.ToString(),
                s.Observaciones,
                s.RespuestaAdmin,
                s.FechaRespuesta
            )).ToList();

            var pagedResult = new PagedResult<SolicitudInsumoDto>(dtos, totalCount, request.PageNumber, request.PageSize);

            return Result<PagedResult<SolicitudInsumoDto>>.Success(pagedResult);
        }
    }
}
