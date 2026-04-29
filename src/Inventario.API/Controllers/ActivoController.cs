using Inventario.Application.Commands.Activos.Create;
using Inventario.Application.Commands.Activos.Delete;
using Inventario.Application.Queries.Activos.Reportes;
using Inventario.Application.Queries.Activos.GetList;
using Inventario.Application.Queries.Activos.Search;
using Inventario.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Inventario.Application.Queries.Activos.Export;
using Inventario.Application.Commands.Activos.Import;
using Inventario.Application.Commands.Activos.Update;

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
            return HandleResult(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateActivoCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id) return BadRequest();
            var result = await Mediator.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Inventario.Application.Common.Pagination.PagedResult<ActivoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? condicion = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] string? categoria = null,
            [FromQuery] string? custodio = null,
            CancellationToken ct = default)
        {
            var query = new GetActivosQuery(page, pageSize, searchTerm, condicion, isActive, categoria, custodio);
            var result = await Mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet("reporte")]
        public async Task<IActionResult> GetReporte(CancellationToken ct)
        {
            var result = await Mediator.Send(new GetActivosReporteQuery(), ct);
            return HandleResult(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<ActivoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string termino, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SearchActivosQuery(termino), cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportToExcel(CancellationToken ct)
        {
            var result = await Mediator.Send(new ExportActivosQuery(), ct);

            if (!result.IsSuccess || result.Value == null)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = $"Activos_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(result.Value, contentType, fileName);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteActivoCommand(id), cancellationToken);
            return HandleResult(result);
        }

        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportExcel(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Archivo no válido");

            // Copiamos a MemoryStream para evitar que el stream se cierre 
            // antes de que el Service pueda leerlo y generar el reporte de error
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);
            ms.Position = 0;

            var command = new ImportActivosCommand(ms, file.FileName);
            var result = await Mediator.Send(command, ct);

            if (!result.Success && result.ErrorFile != null)
            {
                return File(
                    result.ErrorFile,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Errores_{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }

            return Ok(new { result.Procesados, Mensaje = "Importación exitosa" });
        }
    }
}
