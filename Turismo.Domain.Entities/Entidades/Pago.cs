using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Domain.Entities.Entidades
{
    public class Pago : EntidadBase
    {
        

        [Required(ErrorMessage = "La reserva es obligatoria para generar el pago")]
        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; }

        [Required( ErrorMessage = "El monto es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor invalido")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required( ErrorMessage = "El método de pago es obligatorio")]
        [StringLength(50, ErrorMessage = "Solo son permitido 50 carácteres")]
        public string MetodoPago { get; set; }
    }
}
