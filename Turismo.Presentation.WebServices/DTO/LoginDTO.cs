using System.ComponentModel.DataAnnotations;

namespace Turismo.Presentation.WebServices.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El NroDocumento es obligatorio")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordar datos?")]
        public bool RememberMe { get; set; }

        [Required]
        public int TipoUsuario { get; set; }


    }
}
