using Inventario.Application.Commands.Insumos.Create;
using Inventario.Application.Queries.Insumos.GetHistory;
using Inventario.Application.Queries.Insumos.GetPaged;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    public class InsumoController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var query = new GetInsumosPagedQuery(pageNumber, pageSize, searchTerm);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInsumoCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            var query = new GetInsumoHistoryQuery(id);
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
