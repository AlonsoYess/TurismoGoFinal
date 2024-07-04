using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Responses
{
    public class PromocionCreada
    {
        public int EmpresaId { get; set; }
        public string Descripcion { get; set; }
        public decimal Descuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
