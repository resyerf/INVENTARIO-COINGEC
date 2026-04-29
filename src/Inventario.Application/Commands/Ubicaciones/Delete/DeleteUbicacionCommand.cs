using Inventario.Application.Common.Models;
using MediatR;
namespace Inventario.Application.Commands.Ubicaciones.Delete
{
    public record DeleteUbicacionCommand(Guid Id) : IRequest<Result>;
}
