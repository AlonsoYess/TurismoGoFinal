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
    public interface IReseniaService
    {
        Task<ReseniaCreada> CrearReseñaAsync(CrearResenia resenia);
        Task<ReseniaCreada> ObtenerReseñaPorIdAsync(int reseñaId);
        Task<IEnumerable<ReseniaCreada>> ObtenerReseñasPorUsuarioIdAsync(string usuarioId);
        Task<IEnumerable<ReseniaCreada>> ObtenerReseñasPorActividadIdAsync(int actividadId);
        Task<ReseniaCreada> ActualizarReseñaAsync(ActualizarResenia resenia);
        Task<bool> BorrarReseñaAsync(int reseñaId);
        Task<decimal> CalcularCalificacionPromedioPorActividadIdAsync(int actividadId);
        Task<Dictionary<int, decimal>> CalcularCalificacionPromedioPorEmpresaIdAsync(int empresaId);

    }
}
