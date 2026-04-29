using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Activos.Reportes
{
    public record GetActivosReporteQuery() : IRequest<Result<IReadOnlyList<ActivoReporteDto>>>;
}
