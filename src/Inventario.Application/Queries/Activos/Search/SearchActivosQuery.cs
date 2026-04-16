using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Activos.Search
{
    public record SearchActivosQuery(string Termino) : IRequest<IReadOnlyList<ActivoDto>>;
}