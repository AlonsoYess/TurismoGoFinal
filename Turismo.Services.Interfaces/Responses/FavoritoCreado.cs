using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Responses
{
    public class FavoritoCreado
    {        
        public string? Usuario { get; set; }

        public string Actividad { get; set; }

    }
}
