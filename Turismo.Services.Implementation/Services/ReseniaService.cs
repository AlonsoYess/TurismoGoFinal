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
    public class ReseniaService : IReseniaService
    {
        private readonly DBContext _context;

        public ReseniaService(DBContext context)
        {
            _context = context;
        }
        
        public async Task<ReseniaCreada> CrearReseñaAsync(CrearResenia resenia)
        {
            if (resenia == null)
                throw new ArgumentNullException(nameof(resenia));

            ValidarResenia(resenia);

            var usuario = await ObtenerUsuarioPorId(resenia.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {resenia.UsuarioId}.");

            var actividad = await ObtenerActividadPorId(resenia.ActividadId);
            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {resenia.ActividadId}.");

            var nuevaResenia = new Resenia
            {
                UsuarioId = resenia.UsuarioId,
                ActividadId = resenia.ActividadId,
                Calificacion = resenia.Calificacion,
                Comentario = resenia.Comentario,
                FechaReseña = DateTime.Now 
            };

            try
            {
                _context.Resenia.Add(nuevaResenia);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al crear la reseña.", ex);
            }

            return new ReseniaCreada
            {
                Usuario = usuario.Nombre,
                Actividad = actividad.Titulo,
                Calificacion = nuevaResenia.Calificacion,
                Comentario = nuevaResenia.Comentario,
                FechaReseña = nuevaResenia.FechaReseña.ToShortDateString()
            };
        }

        public async Task<ReseniaCreada> ObtenerReseñaPorIdAsync(int reseñaId)
        {
            if (reseñaId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(reseñaId));

            var resenia = await _context.Resenia
                .Include(r => r.Usuario)
                .Include(r => r.Actividad)
                .FirstOrDefaultAsync(r => r.Id == reseñaId);

            if (resenia == null)
                throw new KeyNotFoundException($"No se encontró la reseña con ID {reseñaId}.");

            return new ReseniaCreada
            {
                Usuario = resenia.Usuario.Nombre,
                Actividad = resenia.Actividad.Titulo,
                Calificacion = resenia.Calificacion,
                Comentario = resenia.Comentario,
                FechaReseña = resenia.FechaReseña.ToShortDateString()
            };
        }

        public async Task<IEnumerable<ReseniaCreada>> ObtenerReseñasPorUsuarioIdAsync(string usuarioId)
        {
            var resenias = await _context.Resenia
                .Include(r => r.Usuario)
                .Include(r => r.Actividad)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            return resenias.Select(r => new ReseniaCreada
            {
                Usuario = r.Usuario.Nombre,
                Actividad = r.Actividad.Titulo,
                Calificacion = r.Calificacion,
                Comentario = r.Comentario,
                FechaReseña = r.FechaReseña.ToShortDateString()
            });
        }

        public async Task<IEnumerable<ReseniaCreada>> ObtenerReseñasPorActividadIdAsync(int actividadId)
        {
            var resenias = await _context.Resenia
                .Include(r => r.Usuario)
                .Include(r => r.Actividad)
                .Where(r => r.ActividadId == actividadId)
                .ToListAsync();

            return resenias.Select(r => new ReseniaCreada
            {
                Usuario = r.Usuario.Nombre,
                Actividad = r.Actividad.Titulo,
                Calificacion = r.Calificacion,
                Comentario = r.Comentario,
                FechaReseña = r.FechaReseña.ToShortDateString()
            });
        }

        public async Task<ReseniaCreada> ActualizarReseñaAsync(ActualizarResenia resenia)
        {
            if (resenia == null)
                throw new ArgumentNullException(nameof(resenia));

            ValidarResenia(resenia);

            var reseniaExistente = await _context.Resenia.FindAsync(resenia.Id);
            if (reseniaExistente == null)
                throw new KeyNotFoundException($"No se encontró la reseña con ID {resenia.Id}.");

            var usuario = await ObtenerUsuarioPorId(resenia.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {resenia.UsuarioId}.");

            var actividad = await ObtenerActividadPorId(resenia.ActividadId);
            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {resenia.ActividadId}.");

            reseniaExistente.UsuarioId = resenia.UsuarioId;
            reseniaExistente.ActividadId = resenia.ActividadId;
            reseniaExistente.Calificacion = resenia.Calificacion;
            reseniaExistente.Comentario = resenia.Comentario;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al actualizar la reseña.", ex);
            }

            return new ReseniaCreada
            {
                Usuario = usuario.Nombre,
                Actividad = actividad.Titulo,
                Calificacion = reseniaExistente.Calificacion,
                Comentario = reseniaExistente.Comentario,
                FechaReseña = reseniaExistente.FechaReseña.ToShortDateString()
            };
        }

        public async Task<bool> BorrarReseñaAsync(int reseñaId)
        {
            if (reseñaId <= 0)
                throw new ArgumentException("El ID de la reseña debe ser un número positivo.", nameof(reseñaId));

            var resenia = await _context.Resenia.FindAsync(reseñaId);
            if (resenia == null)
                throw new KeyNotFoundException($"No se encontró la reseña con ID {reseñaId}.");

            try
            {
                _context.Resenia.Remove(resenia);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al borrar la reseña.", ex);
            }

            return true;
        }

        public async Task<decimal> CalcularCalificacionPromedioPorActividadIdAsync(int actividadId)
        {
            if (actividadId <= 0)
                throw new ArgumentException("El ID de la actividad debe ser un número positivo.", nameof(actividadId));

            var promedio = await _context.Resenia
                .Where(r => r.ActividadId == actividadId)
                .AverageAsync(r => r.Calificacion);

            return (decimal)promedio;
        }

        public async Task<Dictionary<int, decimal>> CalcularCalificacionPromedioPorEmpresaIdAsync(int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("El ID de la empresa debe ser un número positivo.", nameof(empresaId));

            var actividades = await _context.Actividad
                .Where(a => a.EmpresaId == empresaId)
                .Include(a => a.Resenia)
                .ToListAsync();

            var promediosPorActividad = new Dictionary<int, decimal>();

            foreach (var actividad in actividades)
            {
                var promedio = actividad.Resenia.Any() ? actividad.Resenia.Average(r => r.Calificacion) : 0;
                promediosPorActividad.Add(actividad.Id, (decimal)promedio);
            }

            return promediosPorActividad;
        }

        private void ValidarResenia(object resenia)
        {
            var context = new ValidationContext(resenia, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(resenia, context, results, true);

            if (!isValid)
            {
                var messages = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException(string.Join("; ", messages));
            }
        }

        private async Task<Usuario> ObtenerUsuarioPorId(string usuarioId)
        {
            return await _context.Users.FindAsync(usuarioId);
        }

        private async Task<Actividad> ObtenerActividadPorId(int actividadId)
        {
            return await _context.Actividad.FindAsync(actividadId);
        }
    }
}
