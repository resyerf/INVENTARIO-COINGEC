using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Dashboard
{
    public sealed record GetDashboardStatsQuery() : IRequest<Result<DashboardStatsDto>>;
}
