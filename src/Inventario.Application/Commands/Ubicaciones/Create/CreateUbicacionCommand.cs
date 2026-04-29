using Inventario.Application.Common.Models;
using MediatR;

namespace Inventario.Application.Commands.Ubicaciones.Create
{
    public record CreateUbicacionCommand(string nombre, string descripcion) : IRequest<Result<Guid>>;
}
