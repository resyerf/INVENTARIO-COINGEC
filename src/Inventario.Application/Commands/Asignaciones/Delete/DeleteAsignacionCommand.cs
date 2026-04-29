using Inventario.Application.Common.Models;
using MediatR;
namespace Inventario.Application.Commands.Asignaciones.Delete
{
    public record DeleteAsignacionCommand(Guid Id) : IRequest<Result>;
}
