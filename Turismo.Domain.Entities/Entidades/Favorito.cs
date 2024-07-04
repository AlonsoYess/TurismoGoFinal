using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Domain.Entities.Entidades
{
    public class Favorito : EntidadBase
    {

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una actividad")]
        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; }

        [Required]
        public DateTime FechaAgregado { get; set; } = DateTime.Now;

    }
}
