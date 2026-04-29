using Inventario.Application.DTOs;
using Inventario.Application.Queries.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public sealed class DashboardController : BaseApiController
    {
        /// <summary>
        /// Obtiene las estadísticas del dashboard
        /// </summary>
        /// <remarks>
        /// Retorna:
        /// - Total de usuarios
        /// - Total de activos
        /// - Activos asignados (sin devolución)
        /// - Activos no asignados
        /// - Total de ubicaciones
        /// - Total de categorías
        /// - Total de subcategorías
        /// </remarks>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(DashboardStatsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboardStats(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetDashboardStatsQuery(), cancellationToken);
            return HandleResult(result);
        }
    }
}
