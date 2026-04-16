using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Activos.GetList
{
    public sealed record GetActivosQuery() : IRequest<IReadOnlyList<ActivoDto>>;
}
