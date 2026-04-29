using Inventario.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();
            return BadRequest(new { error = result.ErrorMessage });
        }

        protected ActionResult HandleResult(Result result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess) return Ok();
            return BadRequest(new { error = result.ErrorMessage });
        }
    }
}