using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Asignaciones.GetList
{
    internal sealed class GetAsignacionesQueryHandler : IRequestHandler<GetAsignacionesQuery, IReadOnlyList<AsignacionDto>>
    {
        private readonly IAsignacionRepository _asignacionRepository;

        public GetAsignacionesQueryHandler(IAsignacionRepository asignacionRepository)
        {
            _asignacionRepository = asignacionRepository ?? throw new ArgumentNullException(nameof(asignacionRepository));
        }

        public async Task<IReadOnlyList<AsignacionDto>> Handle(GetAsignacionesQuery request, CancellationToken cancellationToken)
        {
            var asignaciones = await _asignacionRepository.GetAllWithIncludesAsync(cancellationToken);
            
            return asignaciones.Select(a => new AsignacionDto(
                a.Id,
                a.Activo?.NombreEquipo ?? "Desconocido",
                a.Activo?.Serie ?? "Sin Serie",
                a.Usuario != null ? a.Usuario.NombreCompleto : "Desconocido",
                a.FechaAsignacion,
                a.FechaDevolucion,
                a.EstadoEntrega,
                a.EstadoRecibido,
                a.Observaciones
            )).ToList().AsReadOnly();
        }
    }
}
