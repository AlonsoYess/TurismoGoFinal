using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Requests
{
    public class CrearItinerario
    {

        [Required(ErrorMessage = "Debe seleccionar una actividad")]
        public int ActividadId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el número de día")]
        public int Dia { get; set; }

        [StringLength(255, ErrorMessage = "Paso el límite de caracteres permitidos")]
        public string Descripcion { get; set; }

    }
}
