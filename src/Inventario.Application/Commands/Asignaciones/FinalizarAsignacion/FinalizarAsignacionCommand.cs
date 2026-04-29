
using Inventario.Application.Common.Models;
using MediatR;

namespace Inventario.Application.Commands.Asignaciones.FinalizarAsignacion
{
    public record FinalizarAsignacionCommand(
        Guid AsignacionId,
        string EstadoRecibido,
        string? Observaciones
    ) : IRequest<Result>;
}
