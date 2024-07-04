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
    public interface IReservaService
    {
        Task<ReservaCreada> CrearReservaAsync(CrearReserva reserva);
        Task<ReservaCreada> ObtenerReservaPorIdAsync(int reservaId);
        Task<IEnumerable<ReservaCreada>> ObtenerReservasPorUsuarioIdAsync(string usuarioId);
        Task<IEnumerable<ReservaCreada>> ObtenerReservasPorActividadIdAsync(int actividadId);
        Task<IEnumerable<ReservaCreada>> ObtenerReservasPendientesAsync();
        Task<ReservaCreada> ActualizarEstadoReservaAsync(int reservaId, string estado);
        Task<bool> CancelarReservaAsync(int reservaId);
        Task<bool> BorrarReservaAsync(int reservaId);
    }
}
