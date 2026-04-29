using Inventario.Application.Commands.Categorias.Create;
using Inventario.Application.Commands.Categorias.Delete;
using Inventario.Application.Commands.Categorias.Update;
using Inventario.Application.DTOs;
using Inventario.Application.Queries.Categorias.GetList;
using Inventario.Application.Queries.Categorias.Search;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public sealed class CategoriaController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoriaCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoriaCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id) return BadRequest();
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Inventario.Application.Common.Pagination.PagedResult<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetCategoriasQuery(page, pageSize, searchTerm), cancellationToken);
            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchCategoriasQuery(termino), cancellationToken);
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteCategoriaCommand(id), cancellationToken);
            return NoContent();
        }
    }
}