using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Ubicaciones.Search
{
    public record SearchUbicacionesQuery(string Termino) : IRequest<IReadOnlyList<UbicacionDto>>;
}
