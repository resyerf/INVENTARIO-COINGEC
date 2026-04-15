using Inventario.Application.Commands.Categorias.Create;
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

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            // Enviamos el Query al Mediator
            var result = await Mediator.Send(new GetCategoriasQuery(), cancellationToken);

            // Retornamos la lista mapeada a CategoriaDto
            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchCategoriasQuery(termino), cancellationToken);
            return Ok(result);
        }
    }
}