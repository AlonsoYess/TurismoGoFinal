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
    public class ActividadService : IActividadService
    {
        private readonly DBContext _context;

        public ActividadService(DBContext context)
        {
            _context = context;
        }
        public async Task<ActividadCreada> ActualizarActividadAsync(ActualizarActividad actividad)
        {
            if (actividad == null)
                throw new ArgumentNullException(nameof(actividad));

            ValidarActividad(actividad);

            var actividadExistente = await _context.Actividad.FindAsync(actividad.Id);
            if (actividadExistente == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {actividad.Id}.");

            var empresa = await ObtenerEmpresaPorId(actividad.EmpresaId);
            if (empresa == null)
                throw new KeyNotFoundException($"No se encontró la empresa con ID {actividad.EmpresaId}.");

            actividadExistente.EmpresaId = actividad.EmpresaId;
            actividadExistente.Titulo = actividad.Titulo;
            actividadExistente.Descripcion = actividad.Descripcion;
            actividadExistente.Destino = actividad.Destino;
            actividadExistente.FechaInicio = actividad.FechaInicio;
            actividadExistente.FechaFin = actividad.FechaFin;
            actividadExistente.Precio = actividad.Precio;
            actividadExistente.Capacidad = actividad.Capacidad;

            try
            {
                _context.Entry(actividadExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al actualizar la actividad.", ex);
            }

            return new ActividadCreada
            {
                Id = actividadExistente.Id,
                Empresa = empresa.Nombre,
                EmpresaId = empresa.Id,
                Titulo = actividadExistente.Titulo,
                Descripcion = actividadExistente.Descripcion,
                Destino = actividadExistente.Destino,
                FechaInicio = actividadExistente.FechaInicio,
                FechaFin = actividadExistente.FechaFin,
                Precio = actividadExistente.Precio,
                Capacidad = actividadExistente.Capacidad,
                Reservas = actividadExistente?.Reservas?.Select(r => new ReservaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    FechaReserva = r.FechaReserva.ToString("MM/dd/yyyy"),
                    Cantidad = r.Cantidad,
                    Estado = r.Estado
                }).ToList() ?? new List<ReservaCreada>(),
                Resenia = actividadExistente?.Resenia?.Select(r => new ReseniaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    Calificacion = r.Calificacion,
                    Comentario = r.Comentario,
                    FechaReseña = r.FechaReseña.ToString("MM/dd/yyyy")
                }).ToList() ?? new List<ReseniaCreada>(),
                Itinerarios = actividadExistente?.Itinerarios?.Select(i => new ItinerarioCreado
                {
                    Actividad = actividadExistente.Titulo, // O usar otro campo si es más adecuado
                    Dia = i.Dia,
                    Descripcion = i.Descripcion
                }).ToList() ?? new List<ItinerarioCreado>()
            };
        }

        public async Task<bool> BorrarActividadAsync(int actividadId)
        {
            if (actividadId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(actividadId));

            var actividad = await _context.Actividad.FindAsync(actividadId);
            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {actividadId}.");

            try
            {
                _context.Actividad.Remove(actividad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al eliminar la actividad.", ex);
            }
        }

        public async Task<IEnumerable<ActividadCreada>> BuscarActividadesAsync(string destino, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Actividad
                .Include(a => a.Empresa)
                .Include(a => a.Reservas)
                .Include(a => a.Resenia)
                .Include(a => a.Itinerarios)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(destino))
                query = query.Where(a => a.Destino.Contains(destino));

            if (fechaInicio.HasValue)
                query = query.Where(a => a.FechaInicio >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(a => a.FechaFin <= fechaFin.Value);

            var actividades = await query.ToListAsync();

            return actividades.Select(a => new ActividadCreada
            {
                Empresa = a.Empresa.Nombre,
                Titulo = a.Titulo,
                Descripcion = a.Descripcion,
                Destino = a.Destino,
                FechaInicio = a.FechaInicio,
                FechaFin = a.FechaFin,
                Precio = a.Precio,
                Capacidad = a.Capacidad,
                Reservas = a.Reservas.Select(r => new ReservaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    FechaReserva = r.FechaReserva.ToString("MM/dd/yyyy"),
                    Cantidad = r.Cantidad,
                    Estado = r.Estado
                }).ToList(),
                Resenia = a.Resenia.Select(r => new ReseniaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    Calificacion = r.Calificacion,
                    Comentario = r.Comentario,
                    FechaReseña = r.FechaReseña.ToString("MM/dd/yyyy")
                }).ToList(),
                Itinerarios = a.Itinerarios.Select(i => new ItinerarioCreado
                {
                    //Actividad = actividad.Titulo, 
                    Dia = i.Dia,
                    Descripcion = i.Descripcion
                }).ToList()
            }).ToList();
        }

        public async Task<ActividadCreada> CrearActividadAsync(CrearActividad actividad)
        {
            if (actividad == null)
                throw new ArgumentNullException(nameof(actividad));

            ValidarActividad(actividad);

            var empresa = await ObtenerEmpresaPorId(actividad.EmpresaId);
            if (empresa == null)
                throw new KeyNotFoundException($"No se encontró la empresa con ID {actividad.EmpresaId}.");

            var nuevaActividad = new Actividad
            {
                EmpresaId = actividad.EmpresaId,
                Titulo = actividad.Titulo,
                Descripcion = actividad.Descripcion,
                Destino = actividad.Destino,
                FechaInicio = actividad.FechaInicio,
                FechaFin = actividad.FechaFin,
                Precio = actividad.Precio,
                Capacidad = actividad.Capacidad
            };

            try
            {
                _context.Actividad.Add(nuevaActividad);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al crear la actividad.", ex);
            }

            return new ActividadCreada
            {
                Empresa = empresa.Nombre,
                Titulo = nuevaActividad.Titulo,
                Descripcion = nuevaActividad.Descripcion,
                Destino = nuevaActividad.Destino,
                FechaInicio = nuevaActividad.FechaInicio,
                FechaFin = nuevaActividad.FechaFin,
                Precio = nuevaActividad.Precio,
                Capacidad = nuevaActividad.Capacidad
            };
        }

        public async Task<IEnumerable<ActividadCreada>> ObtenerActividadesPorEmpresaIdAsync(int empresaId)
        {
            throw new NotImplementedException();
        }

        public async Task<ActividadCreada> ObtenerActividadPorIdAsync(int actividadId)
        {
            if (actividadId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(actividadId));

            var actividad = await _context.Actividad
                .Include(a => a.Empresa)
                .Include(a => a.Reservas)
                .Include(a => a.Resenia)
                .Include(a => a.Itinerarios)
                .FirstOrDefaultAsync(a => a.Id == actividadId);

            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {actividadId}.");

            return new ActividadCreada
            {
                Empresa = actividad.Empresa.Nombre,
                Titulo = actividad.Titulo,
                Descripcion = actividad.Descripcion,
                Destino = actividad.Destino,
                FechaInicio = actividad.FechaInicio,
                FechaFin = actividad.FechaFin,
                Precio = actividad.Precio,
                Capacidad = actividad.Capacidad,
                Reservas = actividad.Reservas.Select(r => new ReservaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    FechaReserva = r.FechaReserva.ToString("MM/dd/yyyy"),
                    Cantidad = r.Cantidad,
                    Estado = r.Estado
                    
                }).ToList(),
                Resenia = actividad.Resenia.Select(r => new ReseniaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    Calificacion = r.Calificacion,
                    Comentario = r.Comentario,
                    FechaReseña = r.FechaReseña.ToString("MM/dd/yyyy")
                    
                }).ToList(),
                Itinerarios = actividad.Itinerarios.Select(i => new ItinerarioCreado
                {
                    //Actividad = actividad.Titulo, 
                    Dia = i.Dia,
                    Descripcion = i.Descripcion
                }).ToList()
            };
        }

        public async Task<IEnumerable<ActividadCreada>> ObtenerTodasLasActividadesAsync()
        {
            var actividades = await _context.Actividad
                .Include(a => a.Empresa)
                .Include(a => a.Reservas)
                .Include(a => a.Resenia)
                .Include(a => a.Itinerarios)
                .ToListAsync();

            return actividades.Select(a => new ActividadCreada
            {
                Id = a.Id,
                EmpresaId = a.Empresa.Id,
                Empresa = a.Empresa.Nombre,
                Titulo = a.Titulo,
                Descripcion = a.Descripcion,
                Destino = a.Destino,
                FechaInicio = a.FechaInicio,
                FechaFin = a.FechaFin,
                Precio = a.Precio,
                Capacidad = a.Capacidad,
                Reservas = a.Reservas.Select(r => new ReservaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    FechaReserva = r.FechaReserva.ToString("MM/dd/yyyy"),
                    Cantidad = r.Cantidad,
                    Estado = r.Estado
                }).ToList(),
                Resenia = a.Resenia.Select(r => new ReseniaCreada
                {
                    Usuario = r.Usuario.Nombre,
                    //Actividad = actividad.Titulo,
                    Calificacion = r.Calificacion,
                    Comentario = r.Comentario,
                    FechaReseña = r.FechaReseña.ToString("MM/dd/yyyy")
                }).ToList(),
                Itinerarios = a.Itinerarios.Select(i => new ItinerarioCreado
                {
                    Dia = i.Dia,
                    Descripcion = i.Descripcion
                }).ToList()
            }).ToList();
        }

        private void ValidarActividad(object actividad)
        {
            var context = new ValidationContext(actividad, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(actividad, context, results, true);

            if (!isValid)
            {
                var messages = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException(string.Join("; ", messages));
            }
        }
        private async Task<Empresa> ObtenerEmpresaPorId(int empresaId)
        {
            return await _context.Empresa.FindAsync(empresaId);
        }

    }
}
