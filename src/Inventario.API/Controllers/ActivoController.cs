using Inventario.Application.Commands.Activos.Create;
using Inventario.Application.Queries.Activos.Reportes;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("reporte")]
        public async Task<IActionResult> GetReporte(CancellationToken ct)
        {
            var result = await Mediator.Send(new GetActivosReporteQuery(), ct);
            return Ok(result);
        }
    }
}
