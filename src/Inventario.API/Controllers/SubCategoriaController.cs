using Inventario.Application.Commands.SubCategorias;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public sealed class SubCategoriaController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubCategoriaCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

    }
}
