using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Interfaces
{
    public interface IPagoService
    {
        Task<Pago> CrearPagoAsync(Pago pago);
        Task<Pago> ObtenerPagoPorIdAsync(int pagoId);
        Task<IEnumerable<Pago>> ObtenerPagosPorUsuarioIdAsync(int usuarioId);
        Task<bool> BorrarPagoAsync(int pagoId);
        Task<decimal> ObtenerTotalPagadoPorUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Pago>> ObtenerPagosPorReservaIdAsync(int reservaId);
    }
}
