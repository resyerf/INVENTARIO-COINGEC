using Inventario.Application.Commands.SubCategorias.Create;
using Inventario.Application.Commands.SubCategorias.CreateMasiv;
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
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubCategoriaCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchSubCategoriasQuery(termino), cancellationToken);
            return HandleResult(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteSubCategoriaCommand(id), cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("create-masiv")]
        public async Task<IActionResult> CreateMasiv([FromBody] CreateMasivSubCategoriaCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);
            return HandleResult(result);
        }
    }
}
