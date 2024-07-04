using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Requests
{
    public class CrearActividad
    {

        [Required(ErrorMessage = "Debe seleccionar una empresa")]
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "El campo título es obligatorio")]
        [StringLength(100, ErrorMessage = "Solo son permitido 100 caracteres")]
        public string Titulo { get; set; }

        [StringLength(255, ErrorMessage = "Solo son permitido 255 caracteres")]
        public string Descripcion { get; set; }

        [StringLength(100, ErrorMessage = "Solo son permitido 100 caracteres")]
        public string Destino { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Inicio es obligatorio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El campo Fecha Fin es obligatorio")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El campo Precio obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor Invalido")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo Capacidad es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Valor invalido")]
        public int Capacidad { get; set; }
    }
}
