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
    public class ReservaService : IReservaService
    {
        private readonly DBContext _context;

        public ReservaService(DBContext context)
        {
            _context = context;
        }
        public async Task<ReservaCreada> ActualizarEstadoReservaAsync(int reservaId, string estado)
        {
            if (reservaId <= 0)
                throw new ArgumentException("El ID de la reserva debe ser un número positivo.", nameof(reservaId));

            if (string.IsNullOrEmpty(estado))
                throw new ArgumentException("El estado no puede ser nulo o vacío.", nameof(estado));

            var reservaExistente = await _context.Reserva.FindAsync(reservaId);
            if (reservaExistente == null)
                throw new KeyNotFoundException($"No se encontró la reserva con ID {reservaId}.");

            reservaExistente.Estado = estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al actualizar el estado de la reserva.", ex);
            }

            var usuario = await ObtenerUsuarioPorId(reservaExistente.UsuarioId);
            var actividad = await ObtenerActividadPorId(reservaExistente.ActividadId);

            return new ReservaCreada
            {
                Usuario = usuario.Nombre,
                Actividad = actividad.Titulo,
                FechaReserva = reservaExistente.FechaReserva.ToShortDateString(),
                Cantidad = reservaExistente.Cantidad,
                Estado = reservaExistente.Estado
            };
        }

        public async Task<bool> BorrarReservaAsync(int reservaId)
        {
            if (reservaId <= 0)
                throw new ArgumentException("El ID de la reserva debe ser un número positivo.", nameof(reservaId));

            var reserva = await _context.Reserva.FindAsync(reservaId);
            if (reserva == null)
                throw new KeyNotFoundException($"No se encontró la reserva con ID {reservaId}.");

            try
            {
                _context.Reserva.Remove(reserva);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al borrar la reserva.", ex);
            }

            return true;
        }

        public async Task<bool> CancelarReservaAsync(int reservaId)
        {
            if (reservaId <= 0)
                throw new ArgumentException("El ID de la reserva debe ser un número positivo.", nameof(reservaId));

            var reserva = await _context.Reserva.FindAsync(reservaId);
            if (reserva == null)
                throw new KeyNotFoundException($"No se encontró la reserva con ID {reservaId}.");

            reserva.Estado = "Cancelada";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cancelar la reserva.", ex);
            }

            return true;
        }

        public async Task<ReservaCreada> CrearReservaAsync(CrearReserva reserva)
        {
            if (reserva == null)
                throw new ArgumentNullException(nameof(reserva));

            ValidarReserva(reserva);

            var usuario = await ObtenerUsuarioPorId(reserva.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {reserva.UsuarioId}.");

            var actividad = await ObtenerActividadPorId(reserva.ActividadId);
            if (actividad == null)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {reserva.ActividadId}.");

            var nuevaReserva = new Reserva
            {
                UsuarioId = reserva.UsuarioId,
                ActividadId = reserva.ActividadId,
                FechaReserva = reserva.FechaReserva,
                Cantidad = reserva.Cantidad,
                Estado = reserva.Estado
            };

            try
            {
                _context.Reserva.Add(nuevaReserva);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al crear la reserva.", ex);
            }

            return new ReservaCreada
            {
                Usuario = usuario.Nombre,
                Actividad = actividad.Titulo,
                FechaReserva = nuevaReserva.FechaReserva.ToShortDateString(),
                Cantidad = nuevaReserva.Cantidad,
                Estado = nuevaReserva.Estado
            };
        }

        public async Task<ReservaCreada> ObtenerReservaPorIdAsync(int reservaId)
        {
            if (reservaId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(reservaId));

            var reserva = await _context.Reserva
                .Include(r => r.Usuario)
                .Include(r => r.Actividad)
                .FirstOrDefaultAsync(r => r.Id == reservaId);

            if (reserva == null)
                throw new KeyNotFoundException($"No se encontró la reserva con ID {reservaId}.");

            return new ReservaCreada
            {
                Usuario = reserva.Usuario.Nombre,
                Actividad = reserva.Actividad.Titulo,
                FechaReserva = reserva.FechaReserva.ToShortDateString(),
                Cantidad = reserva.Cantidad,
                Estado = reserva.Estado
            };
        }

        public async Task<IEnumerable<ReservaCreada>> ObtenerReservasPendientesAsync()
        {
            var reservas = await _context.Reserva
            .Include(r => r.Usuario)
            .Include(r => r.Actividad)
            .Where(r => r.Estado == "Pendiente")
            .ToListAsync();

            return reservas.Select(r => new ReservaCreada
            {
                Usuario = r.Usuario.Nombre,
                Actividad = r.Actividad.Titulo,
                FechaReserva = r.FechaReserva.ToShortDateString(),
                Cantidad = r.Cantidad,
                Estado = r.Estado
            });
        }

        public async Task<IEnumerable<ReservaCreada>> ObtenerReservasPorActividadIdAsync(int actividadId)
        {
            if (actividadId <= 0)
                throw new ArgumentException("El ID de la actividad debe ser un número positivo.", nameof(actividadId));

            var reservas = await _context.Reserva
                .Include(r => r.Usuario)
                .Include(r => r.Actividad)
                .Where(r => r.ActividadId == actividadId)
                .ToListAsync();

            return reservas.Select(r => new ReservaCreada
            {
                Usuario = r.Usuario.Nombre,
                Actividad = r.Actividad.Titulo,
                FechaReserva = r.FechaReserva.ToShortDateString(),
                Cantidad = r.Cantidad,
                Estado = r.Estado
            });
        }

        public async Task<IEnumerable<ReservaCreada>> ObtenerReservasPorUsuarioIdAsync(string usuarioId)
        {
            var usuario = await ObtenerUsuarioPorId(usuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {usuarioId}.");

            var reservas = await _context.Reserva
                .Include(r => r.Usuario)
                .Include(r => r.Actividad)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            return reservas.Select(r => new ReservaCreada
            {
                Usuario = r.Usuario.Nombre,
                Actividad = r.Actividad.Titulo,
                ActividadId = r.Actividad.Id,
                FechaReserva = r.FechaReserva.ToShortDateString(),
                Cantidad = r.Cantidad,
                Estado = r.Estado
            });
        }
    

        private void ValidarReserva(object reserva)
        {
            var context = new ValidationContext(reserva, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(reserva, context, results, true);

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
