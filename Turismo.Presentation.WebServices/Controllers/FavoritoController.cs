using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turismo.Services.Interfaces.Interfaces;
using Turismo.Services.Interfaces.Requests;

namespace Turismo.Presentation.WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritoController : ControllerBase
    {

        private readonly IFavoritoService _favoritoService;

        public FavoritoController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        [HttpPost("AgregarFavorito")]
        public async Task<IActionResult> AgregarFavorito([FromBody] CrearFavorito crearFavorito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _favoritoService.AgregarFavoritoAsync(crearFavorito);
                return Ok( resultado);
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message);
            }
        }

        [HttpGet("ObtenerFavoritoPorId/{favoritoId}")]
        public async Task<IActionResult> ObtenerFavoritoPorId(int favoritoId)
        {
            try
            {
                var resultado = await _favoritoService.ObtenerFavoritoPorIdAsync(favoritoId);
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

        [HttpDelete("BorrarFavorito/{favoritoId}")]
        public async Task<IActionResult> BorrarFavorito(int favoritoId)
        {
            try
            {
                var resultado = await _favoritoService.BorrarFavoritoAsync(favoritoId);
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

        [HttpGet("ObtenerFavoritosPorUsuarioId/{usuarioId}")]
        public async Task<IActionResult> ObtenerFavoritosPorUsuarioId(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("El ID del usuario no puede ser nulo o vacío.");
            }

            try
            {
                var resultado = await _favoritoService.ObtenerFavoritosPorUsuarioIdAsync(usuarioId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
