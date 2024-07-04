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
    public interface IEmpresaService
    {
        Task<EmpresaCreada> CrearEmpresaAsync(CrearEmpresa empresa);
        Task<EmpresaCreada> ObtenerEmpresaPorIdAsync(int empresaId);
        Task<List<EmpresaCreada>> ObtenerTodasLasEmpresasAsync();
        Task<EmpresaCreada> ActualizarEmpresaAsync(ActualizarEmpresa empresa);
        Task<bool> BorrarEmpresaAsync(int empresaId);


        Task<List<Actividad>> ObtenerActividadesPorEmpresaAsync(int empresaId);
        
    
    
    }
}
