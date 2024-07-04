using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Requests
{
    public class CrearNotificacion
    {

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? UsuarioId { get; set; }

        [Required(ErrorMessage = "Debe dejar un mensaje")]
        [StringLength(255, ErrorMessage = "Paso el número de carácteres permitidos")]
        public string Mensaje { get; set; }

        [Required]
        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        [Required]
        public bool Leido { get; set; } = false;

    }
}
