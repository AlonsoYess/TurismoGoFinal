using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;
using Turismo.Infraestructure.EFDataContext.Context;
using Turismo.Services.Interfaces.Interfaces;
using Turismo.Services.Interfaces.Requests;
using Turismo.Services.Interfaces.Responses;

namespace Turismo.Services.Implementation.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly DBContext _context;

        public FavoritoService(DBContext context)
        {
            _context = context;
        }
        public async Task<FavoritoCreado> AgregarFavoritoAsync(CrearFavorito favorito)
        {
            if (favorito == null)
                throw new ArgumentNullException(nameof(favorito));

            ValidarFavorito(favorito);

            var usuario = await ObtenerUsuarioPorId(favorito.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {favorito.UsuarioId}.");

            var actividad = await ObtenerActividadPorId(favorito.ActividadId);
            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {favorito.ActividadId}.");

            var nuevoFavorito = new Favorito
            {
                UsuarioId = favorito.UsuarioId,
                ActividadId = favorito.ActividadId
            };

            try
            {
                _context.Favoritos.Add(nuevoFavorito);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al agregar el favorito.", ex);
            }

            return new FavoritoCreado
            {
                Usuario = usuario.Nombre,
                Actividad = actividad.Titulo
            };
        }

        public async  Task<bool> BorrarFavoritoAsync(int favoritoId)
        {
            if (favoritoId <= 0)
                throw new ArgumentException("El ID del favorito debe ser un número positivo.", nameof(favoritoId));

            var favorito = await _context.Favoritos.FindAsync(favoritoId);
            if (favorito == null)
                throw new KeyNotFoundException($"No se encontró el favorito con ID {favoritoId}.");

            try
            {
                _context.Favoritos.Remove(favorito);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al borrar el favorito.", ex);
            }

            return true;
        }

        public async Task<FavoritoCreado> ObtenerFavoritoPorIdAsync(int favoritoId)
        {
            if (favoritoId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(favoritoId));

            var favorito = await _context.Favoritos
                .Include(f => f.Usuario)
                .Include(f => f.Actividad)
                .FirstOrDefaultAsync(f => f.Id == favoritoId);

            if (favorito == null)
                throw new KeyNotFoundException($"No se encontró el favorito con ID {favoritoId}.");

            return new FavoritoCreado
            {
                Usuario = favorito.Usuario.Nombre,
                Actividad = favorito.Actividad.Titulo
            };
        }

        public async Task<IEnumerable<FavoritoCreado>> ObtenerFavoritosPorUsuarioIdAsync(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El ID del usuario no puede ser nulo o vacío.", nameof(usuarioId));

            var favoritos = await _context.Favoritos
                .Include(f => f.Usuario)
                .Include(f => f.Actividad)
                .Where(f => f.UsuarioId == usuarioId)
                .ToListAsync();

            return favoritos.Select(f => new FavoritoCreado
            {
                Usuario = f.Usuario.Nombre,
                Actividad = f.Actividad.Titulo
            });
        }

        private void ValidarFavorito(object favorito)
        {
            var context = new ValidationContext(favorito, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(favorito, context, results, true);

            if (!isValid)
            {
                var messages = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException(string.Join("; ", messages));
            }
        }

        private async Task<Usuario> ObtenerUsuarioPorId(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                throw new ArgumentException("El ID del usuario no puede ser nulo o vacío.", nameof(usuarioId));

            return await _context.Users.FindAsync(usuarioId);
        }

        private async Task<Actividad> ObtenerActividadPorId(int actividadId)
        {
            if (actividadId <= 0)
                throw new ArgumentException("El ID de la actividad debe ser un número positivo.", nameof(actividadId));

            return await _context.Actividad.FindAsync(actividadId);
        }
    }
}
