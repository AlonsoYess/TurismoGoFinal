using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turismo.Services.Interfaces.Interfaces;
using Turismo.Services.Interfaces.Requests;

namespace Turismo.Presentation.WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReseniaController : ControllerBase
    {

        private readonly IReseniaService _reseniaService;

        public ReseniaController(IReseniaService reseniaService)
        {
            _reseniaService = reseniaService;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CrearResenia")]
        public async Task<IActionResult> CrearResenia([FromBody] CrearResenia crearResenia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _reseniaService.CrearReseñaAsync(crearResenia);
                return Ok( resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtenerReseniaPorId/{reseniaId}")]
        public async Task<IActionResult> ObtenerReseniaPorId(int reseniaId)
        {
            try
            {
                var resultado = await _reseniaService.ObtenerReseñaPorIdAsync(reseniaId);
                if (resultado == null)
                {
                    return NotFound();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtenerReseniasPorUsuarioId/{usuarioId}")]
        public async Task<IActionResult> ObtenerReseniasPorUsuarioId(string usuarioId)
        {
            try
            {
                var resultado = await _reseniaService.ObtenerReseñasPorUsuarioIdAsync(usuarioId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtenerReseniasPorActividadId/{actividadId}")]
        public async Task<IActionResult> ObtenerReseniasPorActividadId(int actividadId)
        {
            try
            {
                var resultado = await _reseniaService.ObtenerReseñasPorActividadIdAsync(actividadId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("ActualizarResenia")]
        public async Task<IActionResult> ActualizarResenia([FromBody] ActualizarResenia actualizarResenia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _reseniaService.ActualizarReseñaAsync(actualizarResenia);
                if (resultado == null)
                {
                    return NotFound();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("BorrarResenia/{reseniaId}")]
        public async Task<IActionResult> BorrarResenia(int reseniaId)
        {
            try
            {
                var resultado = await _reseniaService.BorrarReseñaAsync(reseniaId);
                if (!resultado)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CalcularCalificacionPromedioPorActividadId/{actividadId}")]
        public async Task<IActionResult> CalcularCalificacionPromedioPorActividadId(int actividadId)
        {
            try
            {
                var resultado = await _reseniaService.CalcularCalificacionPromedioPorActividadIdAsync(actividadId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CalcularCalificacionPromedioPorEmpresaId/{empresaId}")]
        public async Task<IActionResult> CalcularCalificacionPromedioPorEmpresaId(int empresaId)
        {
            try
            {
                var resultado = await _reseniaService.CalcularCalificacionPromedioPorEmpresaIdAsync(empresaId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
