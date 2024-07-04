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
    public interface IActividadService
    {
        Task<ActividadCreada> CrearActividadAsync(CrearActividad actividad);
        Task<ActividadCreada> ObtenerActividadPorIdAsync(int actividadId);
        Task<IEnumerable<ActividadCreada>> ObtenerTodasLasActividadesAsync();
        Task<IEnumerable<ActividadCreada>> ObtenerActividadesPorEmpresaIdAsync(int empresaId);
        Task<IEnumerable<ActividadCreada>> BuscarActividadesAsync(string destino, DateTime? fechaInicio, DateTime? fechaFin);
        Task<ActividadCreada> ActualizarActividadAsync(ActualizarActividad actividad);
        Task<bool> BorrarActividadAsync(int actividadId);

        //Task<IEnumerable<Actividad>> ObtenerActividadesFavoritasPorUsuarioIdAsync(string usuarioId);
    }
}
