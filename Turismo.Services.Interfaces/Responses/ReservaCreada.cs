using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Responses
{
    public class ReservaCreada
    {
        public string Usuario {  get; set; }
        public string UsuarioId { get; set; }
        public string Actividad { get; set; }
        public int ActividadId { get; set; }
        public string FechaReserva { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }

        
    }
}
