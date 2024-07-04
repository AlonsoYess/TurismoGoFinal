using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Domain.Entities.Entidades
{
    public class Resenia : EntidadBase
    {

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una actividad")]
        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; }

        [Required(ErrorMessage = "Debe poner una Calificacion")]
        [Range(1, 5, ErrorMessage = "Valor invalido")]
        public int Calificacion { get; set; }

        [StringLength(255, ErrorMessage = "Paso el límite de caracteres permitidos")]
        public string Comentario { get; set; }

        [Required]
        public DateTime FechaReseña { get; set; } = DateTime.Now;
    }
}
