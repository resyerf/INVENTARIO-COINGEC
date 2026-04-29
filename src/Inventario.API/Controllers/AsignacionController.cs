using Inventario.API.Models.Requests;
using Inventario.Application.Commands.Asignaciones.Asignar;
using Inventario.Application.Commands.Asignaciones.Delete;
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
        [ProducesResponseType(typeof(Inventario.Application.Common.Pagination.PagedResult<AsignacionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetAsignacionesQuery(page, pageSize, searchTerm), cancellationToken);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AsignacionActivoCommand command, CancellationToken cancellationToken)
        {
            // El resultado es el Guid de la nueva asignación
            var result = await Mediator.Send(command, cancellationToken);
            return HandleResult(result);
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

            var result = await Mediator.Send(command, cancellationToken);
            return HandleResult(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteAsignacionCommand(id), cancellationToken);
            return HandleResult(result);
        }
    }
    
}