using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Responses
{
    public class ReservaCreada
    {
        public string Usuario {  get; set; }
        public string Actividad { get; set; }
        public string FechaReserva { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }
    }
}
