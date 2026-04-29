using Inventario.Application.Common.Models;
using MediatR;
namespace Inventario.Application.Commands.Categorias.Delete
{
    public record DeleteCategoriaCommand(Guid Id) : IRequest<Result>;
}
