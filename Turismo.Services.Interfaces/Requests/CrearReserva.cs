using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Requests
{
    public class CrearReserva
    {

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una actividad")]
        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; }

        [Required(ErrorMessage = "El campo fecha de reserva es obligatorio")]
        public DateTime FechaReserva { get; set; }

        [Required(ErrorMessage = "Es obligatorio seleccionar la cantidad")]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }
    }
}
