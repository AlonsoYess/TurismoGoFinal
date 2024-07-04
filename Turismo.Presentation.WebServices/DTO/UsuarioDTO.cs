namespace Turismo.Presentation.WebServices.DTO
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string NroDocumento { get; set; }
        public string Token { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RefreshToken { get; set; }
    }
}
