using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Asignaciones.GetList
{
    public sealed record GetAsignacionesQuery() : IRequest<IReadOnlyList<AsignacionDto>>;
}
