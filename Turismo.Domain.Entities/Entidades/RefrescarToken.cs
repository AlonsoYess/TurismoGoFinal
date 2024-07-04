using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Domain.Entities.Entidades
{
    public class RefrescarToken
    {

        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsRevoked { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByIp { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        [Required]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
