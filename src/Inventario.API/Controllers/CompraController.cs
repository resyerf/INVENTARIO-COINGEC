using Inventario.Application.Commands.Compras.Registrar;
using Inventario.Application.Queries.Compras.GetPaged;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public class CompraController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var query = new GetComprasPagedQuery(pageNumber, pageSize, searchTerm);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RegistrarCompraCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
