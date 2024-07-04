using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Services.Interfaces.Requests
{
    public class ActualizarEmpresa
    {

        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(100, ErrorMessage = "Solo son permitidos 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo RUC es requerido")]
        [StringLength(11, ErrorMessage = "El RUC solo puede tener 11 caracteres")]
        public string Ruc { get; set; }

        [Required(ErrorMessage = "El campo Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del Email no es válido")]
        [StringLength(100, ErrorMessage = "Solo son permitidos 100 caracteres")]
        public string Email { get; set; }
    }
}
