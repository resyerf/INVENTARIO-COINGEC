using Inventario.Application.Commands.Usuarios.Create;
using Inventario.Application.Commands.Usuarios.Delete;
using Inventario.Application.Queries.Usuarios.GetList;
using Inventario.Application.Queries.Usuarios.Search;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public class UsuarioController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            // Enviamos el Query al Mediator
            var result = await Mediator.Send(new GetUsuariosQuery(), cancellationToken);

            // Retornamos la lista mapeada a UsuarioDto
            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchUsuariosQuery(termino), cancellationToken);
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteUsuarioCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
