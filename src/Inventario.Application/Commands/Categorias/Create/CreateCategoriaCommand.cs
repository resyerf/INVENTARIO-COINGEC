using MediatR;

namespace Inventario.Application.Commands.Categorias.Create
{
    public record CreateCategoriaCommand(
    string Codigo,
    string Descripcion,
    string Valores,
    Guid UbicacionId) : IRequest<Guid>;
}
