using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Domain.Entities.Entidades
{
    public class Usuario : IdentityUser
    {
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }
        public string? NroDocumento { get; set; }

        public int TipoUsuario { get; set; }

        public List<RefrescarToken> RefreshTokens { get; set; }
    }
}
