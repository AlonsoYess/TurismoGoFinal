using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Responses
{
    public class ReseniaCreada
    {
    
        public string Usuario { get; set; }
        public string Actividad { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        public string FechaReseña {  get; set; }

    }
}
