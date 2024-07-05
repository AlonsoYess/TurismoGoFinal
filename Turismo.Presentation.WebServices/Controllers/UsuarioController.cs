using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
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

            var tokenGenerado = await ConstruirToken(usuario);


            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = tokenGenerado.Token,
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
            var usuario = await _userManager.FindByNameAsync(loginDto.Usuario);

            if (usuario == null)
            {
                return Unauthorized();
            }

            if (loginDto.TipoUsuario != usuario.TipoUsuario)
            {
                return Unauthorized();
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, loginDto.Password, false);

            if (!resultado.Succeeded)
            {
                return Unauthorized(resultado);
            }

            

            var tokenGenerado = await ConstruirToken(usuario);

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = tokenGenerado.Token,
                Email = usuario.Email,
                NroDocumento = usuario.UserName
            };
        }


        private async Task<UserToken> ConstruirToken(Usuario userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email),
            };

            var identityUser = await _userManager.FindByEmailAsync(userInfo.Email);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));

            var claimsDB = await _userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };

        }

    }
}
