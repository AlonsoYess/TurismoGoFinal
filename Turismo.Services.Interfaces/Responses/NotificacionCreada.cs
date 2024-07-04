using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Responses
{
    public class NotificacionCreada
    {

        public string? UsuarioId { get; set; }
        public string Mensaje { get; set; }

        public DateTime FechaEnvio { get; set; }

        public bool Leido { get; set; } = false;

    }
}
