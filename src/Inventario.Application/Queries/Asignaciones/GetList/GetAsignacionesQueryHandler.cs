using Inventario.Application.Common.Models;
using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Asignaciones.GetList
{
    internal sealed class GetAsignacionesQueryHandler : IRequestHandler<GetAsignacionesQuery, Result<PagedResult<AsignacionDto>>>
    {
        private readonly IAsignacionRepository _asignacionRepository;

        public GetAsignacionesQueryHandler(IAsignacionRepository asignacionRepository)
        {
            _asignacionRepository = asignacionRepository ?? throw new ArgumentNullException(nameof(asignacionRepository));
        }

        public async Task<Result<PagedResult<AsignacionDto>>> Handle(GetAsignacionesQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _asignacionRepository.GetPagedAsignacionesAsync(
                request.Page, 
                request.PageSize, 
                request.SearchTerm, 
                cancellationToken);
            
            var dtos = items.Select(a => new AsignacionDto(
                a.Id,
                a.Activo?.NombreEquipo ?? "Desconocido",
                a.Activo?.Serie ?? "Sin Serie",
                a.Usuario != null ? a.Usuario.NombreCompleto : "Desconocido",
                a.FechaAsignacion,
                a.FechaDevolucion,
                a.EstadoEntrega,
                a.EstadoRecibido,
                a.Observaciones,
                a.IsActive
            )).ToList().AsReadOnly();

            return Result<PagedResult<AsignacionDto>>.Success(new PagedResult<AsignacionDto>(dtos, totalCount, request.Page, request.PageSize));
        }
    }
}
