using Inventario.Application.Commands.Ubicaciones.Create;
using Inventario.Application.Commands.Ubicaciones.Delete;
using Inventario.Application.Commands.Ubicaciones.Update;
using Inventario.Application.DTOs;
using Inventario.Application.Queries.Ubicaciones.Search;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public sealed class UbicacionController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UbicacionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchUbicacionesQuery(string.Empty), cancellationToken);
            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<UbicacionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchUbicacionesQuery(termino), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUbicacionCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUbicacionCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id) return BadRequest();
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteUbicacionCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
