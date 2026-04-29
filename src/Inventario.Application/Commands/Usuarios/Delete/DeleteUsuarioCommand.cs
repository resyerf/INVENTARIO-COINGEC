using Inventario.Application.Common.Models;
using MediatR;
namespace Inventario.Application.Commands.Usuarios.Delete
{
    public record DeleteUsuarioCommand(Guid Id) : IRequest<Result>;
}
