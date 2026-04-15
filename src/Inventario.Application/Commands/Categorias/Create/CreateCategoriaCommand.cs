using MediatR;

namespace Inventario.Application.Commands.Categorias.Create
{
    public record CreateCategoriaCommand(
    string Codigo,
    string Descripcion,
    Guid UbicacionId) : IRequest<Guid>;
}
