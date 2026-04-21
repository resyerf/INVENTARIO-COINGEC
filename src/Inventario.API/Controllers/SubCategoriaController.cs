using Inventario.Application.Commands.SubCategorias;
using Inventario.Application.Commands.SubCategorias.Delete;
using Inventario.Application.DTOs;
using Inventario.Application.Queries.SubCategorias.GetList;
using Inventario.Application.Queries.SubCategorias.Search;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public sealed class SubCategoriaController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<SubCategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetSubCategoriasQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubCategoriaCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchSubCategoriasQuery(termino), cancellationToken);
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteSubCategoriaCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
