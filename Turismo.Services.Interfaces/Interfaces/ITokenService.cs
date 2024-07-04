using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Interfaces
{
    public interface ITokenService
    {

        Task<(string AccessToken, RefrescarToken RefreshToken)> GenerarToken(Usuario user, string ipAddress, RefrescarToken RefreshToken = null);
        ClaimsPrincipal ValidarToken(string token);
        Task<(string AccessToken, RefrescarToken RefreshToken)> RefrescarToken(string refreshToken, string ipAddress);

    }
}
