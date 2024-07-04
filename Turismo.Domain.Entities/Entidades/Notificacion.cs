using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Domain.Entities.Entidades
{
    public class Notificacion : EntidadBase
    {

        

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required( ErrorMessage = "Debe dejar un mensaje")]
        [StringLength(255, ErrorMessage = "Paso el número de carácteres permitidos")]
        public string Mensaje { get; set; }

        [Required]
        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        [Required]
        public bool Leido { get; set; } = false;

    }
}
