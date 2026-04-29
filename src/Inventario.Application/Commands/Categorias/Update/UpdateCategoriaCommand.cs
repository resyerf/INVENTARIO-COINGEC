using MediatR;

using Inventario.Application.Common.Models;

namespace Inventario.Application.Commands.Categorias.Update
{
    public record UpdateCategoriaCommand(
        Guid Id,
        string Codigo,
        string Descripcion,
        string Valores,
        Guid UbicacionId) : IRequest<Result>;
}
