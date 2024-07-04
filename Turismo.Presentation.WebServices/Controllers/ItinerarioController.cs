using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turismo.Services.Interfaces.Interfaces;
using Turismo.Services.Interfaces.Requests;

namespace Turismo.Presentation.WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItinerarioController : ControllerBase
    {

        private readonly IItinerarioService _itinerarioService;

        public ItinerarioController(IItinerarioService itinerarioService)
        {
            _itinerarioService = itinerarioService;
        }

        [HttpPost("CrearItinerario")]
        public async Task<IActionResult> CrearItinerario([FromBody] CrearItinerario crearItinerario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _itinerarioService.CrearItinerarioAsync(crearItinerario);
                return Ok( resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtenerItinerarioPorId/{itinerarioId}")]
        public async Task<IActionResult> ObtenerItinerarioPorId(int itinerarioId)
        {
            try
            {
                var resultado = await _itinerarioService.ObtenerItinerarioPorIdAsync(itinerarioId);
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

        [HttpPut("ActualizarItinerario")]
        public async Task<IActionResult> ActualizarItinerario([FromBody] ActualizarItinerario actualizarItinerario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _itinerarioService.ActualizarItinerarioAsync(actualizarItinerario);
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

        [HttpDelete("BorrarItinerario/{itinerarioId}")]
        public async Task<IActionResult> BorrarItinerario(int itinerarioId)
        {
            try
            {
                var resultado = await _itinerarioService.BorrarItinerarioAsync(itinerarioId);
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

        [HttpGet("ObtenerItinerariosPorActividadId/{actividadId}")]
        public async Task<IActionResult> ObtenerItinerariosPorActividadId(int actividadId)
        {
            try
            {
                var resultado = await _itinerarioService.ObtenerItinerariosPorActividadIdAsync(actividadId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ValidarItinerarioPorFechas/{actividadId}/{dia}")]
        public async Task<IActionResult> ValidarItinerarioPorFechas(int actividadId, int dia)
        {
            try
            {
                var resultado = await _itinerarioService.ValidarItinerarioPorFechasAsync(actividadId, dia);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
