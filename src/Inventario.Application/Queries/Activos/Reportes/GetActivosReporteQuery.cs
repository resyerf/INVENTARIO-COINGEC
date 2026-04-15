using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Activos.Reportes
{
    public record GetActivosReporteQuery() : IRequest<IReadOnlyList<ActivoReporteDto>>;
}
