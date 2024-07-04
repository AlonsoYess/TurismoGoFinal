using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Requests
{
    public class CrearFavorito
    {

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? UsuarioId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una actividad")]
        public int ActividadId { get; set; }
        

    }
}
