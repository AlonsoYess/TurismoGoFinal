using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turismo.Services.Interfaces.Interfaces;
using Turismo.Services.Interfaces.Requests;
using Turismo.Services.Interfaces.Responses;

namespace Turismo.Presentation.WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        [HttpPost]
        [Route("CrearEmpresaAsync")]
        public async Task<ActionResult<EmpresaCreada>> CrearEmpresaAsync([FromBody] CrearEmpresa _empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var empresaCreada = await _empresaService.CrearEmpresaAsync(_empresa);
                return Ok(empresaCreada);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut]
        [Route("ActualizarEmpresaAsync")]
        public async Task<ActionResult<EmpresaCreada>> ActualizarEmpresaAsync([FromBody] ActualizarEmpresa _empresa)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var empresaActualizada = await _empresaService.ActualizarEmpresaAsync(_empresa);
                return Ok(empresaActualizada);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.InnerException?.Message ?? ex.Message}");
            }

        }

        [HttpGet]
        [Route("ObtenerEmpresaPorIdAsync/{empresaId}")]
        public async Task<ActionResult<EmpresaCreada>> ObtenerEmpresaPorIdAsync(int empresaId)
        {
            try
            {
                var empresa = await _empresaService.ObtenerEmpresaPorIdAsync(empresaId);

                return Ok(empresa);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error : {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        
        [HttpGet]
        [Route("ObtenerTodasLasEmpresasAsync")]
        
        public async Task<ActionResult<List<EmpresaCreada>>> ObtenerTodasLasEmpresasAsync()
        {
            try
            {
                var empresas = await _empresaService.ObtenerTodasLasEmpresasAsync();
                return empresas;
                //return Ok(empresas);
            }
            catch (Exception ex)
            {
                return BadRequest( $"Error : {ex.InnerException?.Message ?? ex.Message}");
            }
        }




    }
}
