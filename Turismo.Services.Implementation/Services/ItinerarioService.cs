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
    public class ItinerarioService :IItinerarioService
    {
        private readonly DBContext _context;

        public ItinerarioService(DBContext context)
        {
            _context = context;
        }

        public async Task<ItinerarioCreado> ActualizarItinerarioAsync(ActualizarItinerario itinerario)
        {
            if (itinerario == null)
                throw new ArgumentNullException(nameof(itinerario));

            ValidarItinerario(itinerario);

            var itinerarioExistente = await _context.Itinerario.FindAsync(itinerario.Id);
            if (itinerarioExistente == null)
                throw new KeyNotFoundException($"No se encontró el itinerario con ID {itinerario.Id}.");

            var actividad = await _context.Actividad.FindAsync(itinerario.ActividadId);
            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {itinerario.ActividadId}.");

            itinerarioExistente.ActividadId = itinerario.ActividadId;
            itinerarioExistente.Dia = itinerario.Dia;
            itinerarioExistente.Descripcion = itinerario.Descripcion;

            try
            {
                _context.Entry(itinerarioExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al actualizar el itinerario.", ex);
            }

            return new ItinerarioCreado
            {
                
                Actividad = actividad.Titulo,
                Dia = itinerarioExistente.Dia,
                Descripcion = itinerarioExistente.Descripcion
            };
        }

        public async Task<bool> BorrarItinerarioAsync(int itinerarioId)
        {
            if (itinerarioId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(itinerarioId));

            var itinerario = await _context.Itinerario.FindAsync(itinerarioId);
            if (itinerario == null)
                throw new KeyNotFoundException($"No se encontró el itinerario con ID {itinerarioId}.");

            try
            {
                _context.Itinerario.Remove(itinerario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al borrar el itinerario.", ex);
            }

            return true;
        }

        public async Task<ItinerarioCreado> CrearItinerarioAsync(CrearItinerario itinerario)
        {

            if (itinerario == null)
                throw new ArgumentNullException(nameof(itinerario));

            ValidarItinerario(itinerario);

            var actividad = await _context.Actividad.FindAsync(itinerario.ActividadId);
            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {itinerario.ActividadId}.");

            var nuevoItinerario = new Itinerario
            {
                ActividadId = itinerario.ActividadId,
                Dia = itinerario.Dia,
                Descripcion = itinerario.Descripcion
            };

            try
            {
                _context.Itinerario.Add(nuevoItinerario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al crear el itinerario.", ex);
            }

            return new ItinerarioCreado
            {
                
                Actividad = actividad.Titulo,
                Dia = nuevoItinerario.Dia,
                Descripcion = nuevoItinerario.Descripcion
            };

        }

        public async Task<ItinerarioCreado> ObtenerItinerarioPorIdAsync(int itinerarioId)
        {
            if (itinerarioId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(itinerarioId));

            var itinerario = await _context.Itinerario.FindAsync(itinerarioId);
            if (itinerario == null)
                throw new KeyNotFoundException($"No se encontró el itinerario con ID {itinerarioId}.");

            return new ItinerarioCreado
            {
                
                Actividad = itinerario.Actividad.Titulo,
                Dia = itinerario.Dia,
                Descripcion = itinerario.Descripcion
            };
        }

        public Task<IEnumerable<ItinerarioCreado>> ObtenerItinerariosPorActividadIdAsync(int actividadId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidarItinerarioPorFechasAsync(int actividadId, int dia)
        {
            throw new NotImplementedException();
        }

        private void ValidarItinerario(object itinerario)
        {
            var context = new ValidationContext(itinerario, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(itinerario, context, results, true);

            if (!isValid)
            {
                var messages = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException(string.Join("; ", messages));
            }
        }

        
    }
}
