using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Turismo.Domain.Entities.Entidades;
using Turismo.Presentation.WebServices.DTO;
using Turismo.Services.Interfaces.Interfaces;

namespace Turismo.Presentation.WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ITokenService _tokenService;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDTO>> Registrar(RegistrarUsuarioDTO registrarUsuarioDTO)
        {

            var usuario = new Usuario
            {
                Email = registrarUsuarioDTO.Email,
                UserName = registrarUsuarioDTO.NroDocumento,
                Nombre = registrarUsuarioDTO.Nombre,
                Apellido = registrarUsuarioDTO.Apellido,
                NroDocumento = registrarUsuarioDTO.NroDocumento
            };

            var resultado = await _userManager.CreateAsync(usuario, registrarUsuarioDTO.Password);

            if (!resultado.Succeeded)
            {
                //ValidarErrores(resultado);
                return BadRequest(resultado.Errors);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Generar el token de acceso y el refresh token
            var (accessToken, refreshToken) = await _tokenService.GenerarToken(usuario, ipAddress);

            // Asocia el refresh token al usuario
            usuario.RefreshTokens.Add(refreshToken);


            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = accessToken,
                Email = usuario.Email,
                NroDocumento = usuario.NroDocumento
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> Login(LoginDTO loginDto)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }*/
            //var usuario = await _userManager.FindByEmailAsync(loginDto.Email);
            var usuario = await _userManager.FindByNameAsync(loginDto.NroDocumento);
            if (usuario == null)
            {
                return Unauthorized();
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, loginDto.Password, false);

            if (!resultado.Succeeded)
            {
                return Unauthorized(resultado);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var (accessToken, refreshToken) = await _tokenService.GenerarToken(usuario, ipAddress);

            // Asocia el refresh token al usuario
            usuario.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(usuario);


            // Configura la cookie para el token de acceso
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true, // Hace que la cookie no sea accesible desde JavaScript
                Secure = true,   // Solo si estás utilizando HTTPS
                SameSite = SameSiteMode.Strict, // Configura SameSite según tus necesidades
                                                // Otras opciones de configuración de cookies según tus necesidades
            });

            // Configura la cookie para el token de acceso
            Response.Cookies.Append("refresh_token", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true, // Hace que la cookie no sea accesible desde JavaScript
                Secure = true,   // Solo si estás utilizando HTTPS
                SameSite = SameSiteMode.Strict, // Configura SameSite según tus necesidades
                                                // Otras opciones de configuración de cookies según tus necesidades
            });

            //var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = accessToken,
                Email = usuario.Email,
                NroDocumento = usuario.UserName,
                RefreshToken = refreshToken.Token
            };
        }

    }
}
