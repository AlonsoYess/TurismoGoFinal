using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Requests
{
    public class CrearPromocion
    {

        [Required(ErrorMessage = "La empresa es obligatoria")]
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "El campo descripcion es obligatorio")]
        [StringLength(255, ErrorMessage = "Paso el limite permitido de caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El descuento es obligatorio")]
        [Range(0, 100, ErrorMessage = "Valor invalido")]
        public decimal Descuento { get; set; }

        [Required(ErrorMessage = "La fecha de Inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La Fecha Fin es obligatoria")]
        public DateTime FechaFin { get; set; }
    }
}
