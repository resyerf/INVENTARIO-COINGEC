using Inventario.API.Models.Requests;
using Inventario.Application.Commands.Asignaciones.Asignar;
using Inventario.Application.Commands.Asignaciones.FinalizarAsignacion;
using Inventario.Application.Queries.Asignaciones.GetList;
using Inventario.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    [Route("api/[controller]")]
    public class AsignacionController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<AsignacionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetAsignacionesQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AsignacionActivoCommand command, CancellationToken cancellationToken)
        {
            // El resultado es el Guid de la nueva asignación
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/finalizar")]
        public async Task<IActionResult> Finalize(Guid id, [FromBody] FinalizarAsignacionRequest request, CancellationToken cancellationToken)
        {
            // Mapeamos el ID de la URL y el body al Command
            var command = new FinalizarAsignacionCommand(
                id,
                request.EstadoRecibido,
                request.Observaciones
            );

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
    
}