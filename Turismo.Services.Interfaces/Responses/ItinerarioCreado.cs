using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Responses
{
    public class ItinerarioCreado
    {
        public int Id { get; set; }
        public string Actividad { get; set; }
        public int ActividadId { get; set; }
        public int Dia { get; set; }
        public string Descripcion { get; set; }
    }
}
