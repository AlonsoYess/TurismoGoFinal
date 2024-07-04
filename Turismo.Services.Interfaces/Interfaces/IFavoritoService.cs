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
    public interface IFavoritoService
    {

        Task<FavoritoCreado> AgregarFavoritoAsync(CrearFavorito favorito);
        Task<FavoritoCreado> ObtenerFavoritoPorIdAsync(int favoritoId);
        Task<IEnumerable<FavoritoCreado>> ObtenerFavoritosPorUsuarioIdAsync(string usuarioId);
        Task<bool> BorrarFavoritoAsync(int favoritoId);
        
    }
}
