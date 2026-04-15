using Inventario.Application.Commands.Categorias.Create;
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

        // Aquí irían los otros endpoints (GetList, GetById, etc.)
    }
}