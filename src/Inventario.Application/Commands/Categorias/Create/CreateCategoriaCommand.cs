using MediatR;

using Inventario.Application.Common.Models;

namespace Inventario.Application.Commands.Categorias.Create
{
    public record CreateCategoriaCommand(
    string Codigo,
    string Descripcion,
    string Valores,
    Guid UbicacionId) : IRequest<Result<Guid>>;
}
