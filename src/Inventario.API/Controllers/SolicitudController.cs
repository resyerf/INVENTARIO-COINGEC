using Inventario.Application.Commands.Solicitudes.Aprobar;
using Inventario.Application.Commands.Solicitudes.Crear;
using Inventario.Application.Commands.Solicitudes.Rechazar;
using Inventario.Application.Queries.Solicitudes.GetPaged;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public class SolicitudController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var query = new GetSolicitudesPagedQuery(pageNumber, pageSize, searchTerm);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearSolicitudCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("aprobar")]
        public async Task<IActionResult> Aprobar([FromBody] AprobarSolicitudCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("rechazar")]
        public async Task<IActionResult> Rechazar([FromBody] RechazarSolicitudCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
