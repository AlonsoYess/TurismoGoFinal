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
    public interface IPromocionService
    {
        Task<PromocionCreada> CrearPromocionAsync(CrearPromocion promocion);
        Task<PromocionCreada> ObtenerPromocionPorIdAsync(int promocionId);
        Task<IEnumerable<PromocionCreada>> ObtenerTodasLasPromocionesAsync();
        Task<IEnumerable<PromocionCreada>> ObtenerPromocionesVigentesAsync();
        Task<PromocionCreada> ActualizarPromocionAsync(CrearPromocion promocion);
        Task<bool> BorrarPromocionAsync(int promocionId);
        Task<List<PromocionCreada>> ObtenerPromocionesPorEmpresaAsync(int empresaId);
    }
}
