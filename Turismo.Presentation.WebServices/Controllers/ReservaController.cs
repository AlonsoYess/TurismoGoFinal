using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turismo.Services.Interfaces.Interfaces;
using Turismo.Services.Interfaces.Requests;

namespace Turismo.Presentation.WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }


        [HttpPost("CrearReserva")]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReserva crearReserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _reservaService.CrearReservaAsync(crearReserva);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("ObtenerReservaPorId/{reservaId}")]
        public async Task<IActionResult> ObtenerReservaPorId(int reservaId)
        {
            try
            {
                var resultado = await _reservaService.ObtenerReservaPorIdAsync(reservaId);
                if (resultado == null)
                {
                    return NotFound();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message);
            }
        }

        [HttpGet("ObtenerReservasPorUsuarioId/{usuarioId}")]
        public async Task<IActionResult> ObtenerReservasPorUsuarioId(string usuarioId)
        {
            try
            {
                var resultado = await _reservaService.ObtenerReservasPorUsuarioIdAsync(usuarioId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtenerReservasPorActividadId/{actividadId}")]
        public async Task<IActionResult> ObtenerReservasPorActividadId(int actividadId)
        {
            try
            {
                var resultado = await _reservaService.ObtenerReservasPorActividadIdAsync(actividadId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtenerReservasPendientes")]
        public async Task<IActionResult> ObtenerReservasPendientes()
        {
            try
            {
                var resultado = await _reservaService.ObtenerReservasPendientesAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ActualizarEstadoReserva/{reservaId}")]
        public async Task<IActionResult> ActualizarEstadoReserva(int reservaId, [FromBody] string estado)
        {
            if (string.IsNullOrEmpty(estado))
            {
                return BadRequest("El estado no puede ser nulo o vacío.");
            }

            try
            {
                var resultado = await _reservaService.ActualizarEstadoReservaAsync(reservaId, estado);
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

        [HttpPut("CancelarReserva/{reservaId}")]
        public async Task<IActionResult> CancelarReserva(int reservaId)
        {
            try
            {
                var resultado = await _reservaService.CancelarReservaAsync(reservaId);
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

        [HttpDelete("BorrarReserva/{reservaId}")]
        public async Task<IActionResult> BorrarReserva(int reservaId)
        {
            try
            {
                var resultado = await _reservaService.BorrarReservaAsync(reservaId);
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


    }
}
