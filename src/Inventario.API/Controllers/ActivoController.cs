using Inventario.Application.Commands.Activos.Create;
using Inventario.Application.Queries.Activos.Reportes;
using Inventario.Application.Queries.Activos.GetList;
using Inventario.Application.Queries.Activos.Search;
using Inventario.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Inventario.Application.Queries.Activos.Export;

namespace Inventario.API.Controllers
{
    public class ActivoController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateActivoCommand command, CancellationToken cancellationToken)
        {
            // El validador de FluentValidation se ejecuta automáticamente antes de entrar aquí 
            // si tienes configurado el ValidationBehavior en MediatR.
            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ActivoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await Mediator.Send(new GetActivosQuery(), ct);
            return Ok(result);
        }

        [HttpGet("reporte")]
        public async Task<IActionResult> GetReporte(CancellationToken ct)
        {
            var result = await Mediator.Send(new GetActivosReporteQuery(), ct);
            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<ActivoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchActivosQuery(termino), cancellationToken);
            return Ok(result);
        }

        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportToExcel(CancellationToken ct)
        {
            var resultBytes = await Mediator.Send(new ExportActivosQuery(), ct);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = $"Activos_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(resultBytes, contentType, fileName);
        }
    }
}
