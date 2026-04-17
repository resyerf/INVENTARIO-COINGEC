using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Dashboard
{
    public sealed record GetDashboardStatsQuery() : IRequest<DashboardStatsDto>;
}
