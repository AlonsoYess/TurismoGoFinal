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
    public interface IItinerarioService
    {
        Task<ItinerarioCreado> CrearItinerarioAsync(CrearItinerario itinerario);
        Task<ItinerarioCreado> ObtenerItinerarioPorIdAsync(int itinerarioId);
        Task<IEnumerable<ItinerarioCreado>> ObtenerItinerariosPorActividadIdAsync(int actividadId);
        Task<ItinerarioCreado> ActualizarItinerarioAsync(ActualizarItinerario itinerario);
        Task<bool> BorrarItinerarioAsync(int itinerarioId);
        Task<bool> ValidarItinerarioPorFechasAsync(int actividadId, int dia);
    }
}
