using System.ComponentModel.DataAnnotations;

namespace Turismo.Presentation.WebServices.DTO
{
    public class RegistrarUsuarioDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato incorrecto para correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(8, ErrorMessage = "Debe tener como mínimo 8 carácteres")]
        [MaxLength(11, ErrorMessage = "Debe tener como mínimo 11 carácteres")]
        public string NroDocumento { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo contraseña es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo confirmar contraseña es requerido")]
        [Compare("Password", ErrorMessage = "La contraseña y confirmación de contraseña no coinciden")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
