using MediatR;

namespace Inventario.Application.Commands.Categorias.Update
{
    public record UpdateCategoriaCommand(
        Guid Id,
        string Codigo,
        string Descripcion,
        string Valores,
        Guid UbicacionId) : IRequest<Unit>;
}
