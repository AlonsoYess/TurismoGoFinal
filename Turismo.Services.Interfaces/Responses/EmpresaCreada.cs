using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Responses
{
    public class EmpresaCreada
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ruc { get; set; }

        public string Email { get; set; }

        public List<Actividad> Actividades { get; set; }
    }
}
