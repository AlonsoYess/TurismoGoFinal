using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turismo.Presentation.WebServices.DTO;
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

        private async Task<string> GuardarImagenAsync(IFormFile imagen)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "imagenes");
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imagen.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            return fileName;
        }

        [HttpPost("CrearActividad")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CrearActividad([FromForm] CrearActividadDTO crearActividad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var imagenNombre = await GuardarImagenAsync(crearActividad.Imagen);

                var actividadFinal = new CrearActividad {
                    EmpresaId = crearActividad.EmpresaId,
                    Titulo = crearActividad.Titulo,
                    Descripcion = crearActividad.Descripcion,
                    Destino = crearActividad.Destino,
                    FechaInicio = crearActividad.FechaInicio,
                    FechaFin = crearActividad.FechaFin,
                    Precio = crearActividad.Precio,
                    Capacidad = crearActividad.Capacidad,
                    Imagen = imagenNombre
                };

                var resultado = await _actividadService.CrearActividadAsync(actividadFinal);
                return Ok( resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ActualizarActividad")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizarActividad([FromForm] ActualizarActividadDTO actualizarActividad)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string imagenNombre = string.Empty;
                if (actualizarActividad.Imagen != null)
                {
                     imagenNombre = await GuardarImagenAsync(actualizarActividad.Imagen);
                }
                else
                {
                    imagenNombre = actualizarActividad.ImagenAnterior;
                }

                var actividadFinal = new ActualizarActividad
                {
                    Id = actualizarActividad.Id,
                    EmpresaId = actualizarActividad.EmpresaId,
                    Titulo = actualizarActividad.Titulo,
                    Descripcion = actualizarActividad.Descripcion,
                    Destino = actualizarActividad.Destino,
                    FechaInicio = actualizarActividad.FechaInicio,
                    FechaFin = actualizarActividad.FechaFin,
                    Precio = actualizarActividad.Precio,
                    Capacidad = actualizarActividad.Capacidad,
                    Imagen = imagenNombre
                };

                var resultado = await _actividadService.ActualizarActividadAsync(actividadFinal);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
