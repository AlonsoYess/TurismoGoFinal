using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;
using Turismo.Infraestructure.EFDataContext.Context;
using Turismo.Services.Interfaces.Interfaces;

namespace Turismo.Services.Implementation.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Usuario> _userManager;
        private readonly RSA _rsa;
        private readonly IConfigurationSection _tokenSettings;
        private readonly DBContext _context;

        public TokenService(IConfiguration configuration, UserManager<Usuario> userManager, DBContext context, RSA rsa)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _tokenSettings = _configuration.GetSection("Token");

            // Crear instancia RSA
            _rsa = rsa;
        }

        public async Task<(string AccessToken, RefrescarToken RefreshToken)> GenerarToken(Usuario user, string ipAddress, RefrescarToken RefreshToken = null)
        {
            var issuer = _tokenSettings["Issuer"];
            var audience = _tokenSettings["Audience"];

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var rsaKey = new RsaSecurityKey(_rsa);
            rsaKey.KeyId = Guid.NewGuid().ToString();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256),
                Issuer = issuer,
                Audience = audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            RefrescarToken refreshToken;

            if (RefreshToken == null)
            {
                refreshToken = GenerarRefreshToken(ipAddress);

                // store refresh token
                refreshToken.UsuarioId = user.Id;
                _context.RefrescarToken.Add(refreshToken);
                await _context.SaveChangesAsync();
            }
            else
            {
                refreshToken = RefreshToken;
            }
            // Genera el token de refresco.


            return (tokenHandler.WriteToken(securityToken), refreshToken);
        }


        public ClaimsPrincipal ValidarToken(string token)
        {
            var issuer = _tokenSettings["Issuer"];
            var audience = _tokenSettings["Audience"];

            var rsaKey = new RsaSecurityKey(_rsa);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = rsaKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = false,
                //ClockSkew = TimeSpan.Zero,
                //RequireExpirationTime = true,
                RequireSignedTokens = true,
                TokenDecryptionKey = rsaKey,
                ValidateTokenReplay = true,
                SaveSigninToken = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Token Invalido");
                }
                return principal;
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                // La firma del token no es válida
                throw new SecurityTokenException("La firma del token no es válida");
            }
            catch (SecurityTokenExpiredException ex)
            {
                // El token ha caducado, puedes proporcionar un mensaje de error específico
                throw new SecurityTokenException("El token ha caducado");
            }
            catch (SecurityTokenException ex)
            {
                throw new SecurityTokenException("Token Invalido ", ex);
            }
        }


        public async Task<(string AccessToken, RefrescarToken RefreshToken)> RefrescarToken(string refreshToken, string ipAddress)
        {
            var usuario = _context.Users
                      .Include(u => u.RefreshTokens)
                      .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));


            if (usuario == null) throw new Exception("Token invalido 1 + RefrescarToken");

            var refreshTokenEntity = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

            /*if (!refreshTokenEntity.IsExpired)
            {
                throw new Exception("Token invalido 2 + RefrescarToken");
            }*/

            if (refreshTokenEntity.RevokedDate != null)
            {
                throw new Exception("Token invalido 3 + RefrescarToken");
            }

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = GenerarRefreshToken(ipAddress);
            refreshTokenEntity.RevokedDate = DateTime.UtcNow;
            refreshTokenEntity.RevokedByIp = ipAddress;
            refreshTokenEntity.ReplacedByToken = newRefreshToken.Token;
            newRefreshToken.UsuarioId = usuario.Id;
            _context.RefrescarToken.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            // generate new jwt
            var jwtToken = await GenerarToken(usuario, ipAddress, newRefreshToken);

            return (jwtToken.AccessToken, newRefreshToken);
        }

        private static RefrescarToken GenerarRefreshToken(string ipAddress)
        {
            var refreshToken = new RefrescarToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                ExpiryDate = DateTime.UtcNow.AddMinutes(3),
                CreatedDate = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }
    }
}
