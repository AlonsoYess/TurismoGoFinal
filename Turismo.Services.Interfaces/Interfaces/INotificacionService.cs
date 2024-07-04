using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;
using Turismo.Services.Interfaces.Requests;
using Turismo.Services.Interfaces.Responses;

namespace Turismo.Services.Interfaces.Interfaces
{
    public interface INotificacionService
    {

        Task<NotificacionCreada> CrearNotificacionAsync(CrearNotificacion notificacion);
        Task<NotificacionCreada> ObtenerNotificacionPorIdAsync(int notificacionId);
        Task<List<NotificacionCreada>> ObtenerNotificacionesPorUsuarioIdAsync(int usuarioId);
        Task<NotificacionCreada> MarcarComoLeidaAsync(int notificacionId);
        Task<bool> BorrarNotificacionAsync(int notificacionId);
        Task<List<NotificacionCreada>> ObtenerNotificacionesNoLeidasPorUsuarioIdAsync(int usuarioId);
    }
}
