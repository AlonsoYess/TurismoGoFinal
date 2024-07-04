using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);
        Task<Usuario> ObtenerUsuarioPorIdAsync(int usuarioId);
        Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync();
        Task<Usuario> ActualizarUsuarioAsync(Usuario usuario);
        Task<bool> BorrarUsuarioAsync(int usuarioId);
        Task<IEnumerable<Reserva>> ObtenerHistorialReservasAsync(int usuarioId);
        Task<IEnumerable<Actividad>> ObtenerActividadesFavoritasAsync(int usuarioId);

    }
}
