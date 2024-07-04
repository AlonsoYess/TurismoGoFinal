using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Requests
{
    public class CrearEmpresa
    {
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(100, ErrorMessage = "Solo son permitido 100 carácteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Ruc es requerido")]
        [StringLength(11, ErrorMessage = "El ruc solo puede tener 11 caracteres")]
        public string Ruc { get; set; }

        [Required(ErrorMessage = "El campo Email es obligatorio")]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Solo son permitido 100 caracteres")]
        public string Email { get; set; }
    }
}
