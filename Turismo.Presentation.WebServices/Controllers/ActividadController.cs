using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turismo.Services.Interfaces.Interfaces;
using Turismo.Services.Interfaces.Requests;

namespace Turismo.Presentation.WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : ControllerBase
    {
        private readonly IActividadService _actividadService;

        public ActividadController(IActividadService actividadService)
        {
            _actividadService = actividadService;
        }

        [HttpPost("CrearActividad")]
        public async Task<IActionResult> CrearActividad([FromBody] CrearActividad crearActividad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _actividadService.CrearActividadAsync(crearActividad);
                return Ok( resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ActualizarActividad")]
        public async Task<IActionResult> ActualizarActividad([FromBody] ActualizarActividad actualizarActividad)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _actividadService.ActualizarActividadAsync(actualizarActividad);
                return Ok(resultado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("BorrarActividad/{id}")]
        public async Task<IActionResult> BorrarActividad(int id)
        {
            try
            {
                var resultado = await _actividadService.BorrarActividadAsync(id);
                if (!resultado)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ObtenerActividadPorId/{id}")]
        public async Task<IActionResult> ObtenerActividadPorId(int id)
        {
            try
            {
                var resultado = await _actividadService.ObtenerActividadPorIdAsync(id);
                if (resultado == null)
                {
                    return NotFound();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ObtenerActividadesPorEmpresaId/{empresaId}")]
        public async Task<IActionResult> ObtenerActividadesPorEmpresaId(int empresaId)
        {
            try
            {
                var resultado = await _actividadService.ObtenerActividadesPorEmpresaIdAsync(empresaId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarActividades([FromQuery] string destino, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var resultado = await _actividadService.BuscarActividadesAsync(destino, fechaInicio, fechaFin);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ObtenerTodasLasActividades")]
        public async Task<IActionResult> ObtenerTodasLasActividades()
        {
            try
            {
                var resultado = await _actividadService.ObtenerTodasLasActividadesAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
