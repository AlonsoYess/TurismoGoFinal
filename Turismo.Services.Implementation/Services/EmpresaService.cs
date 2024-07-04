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
    public class EmpresaService : IEmpresaService
    {
        private readonly DBContext _context;

        public EmpresaService(DBContext context)
        {
            _context = context;
        }

        public async Task<EmpresaCreada> ActualizarEmpresaAsync(ActualizarEmpresa _empresa)
        {
            if (_empresa == null)
                throw new ArgumentNullException(nameof(_empresa));

            ValidarEmpresa(_empresa);

            var empresaExistente = await _context.Empresa.FindAsync(_empresa.Id);
            if (empresaExistente == null)
                throw new KeyNotFoundException($"No se encontró la empresa con ID {_empresa.Id}.");

            if (await EmpresaExists(_empresa.Nombre, _empresa.Ruc, _empresa.Id))
                throw new InvalidOperationException("Una empresa con el mismo nombre o RUC ya existe.");

            empresaExistente.Nombre = _empresa.Nombre;
            empresaExistente.Ruc = _empresa.Ruc;
            empresaExistente.Email = _empresa.Email;
            

            try
            {
                _context.Entry(empresaExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al actualizar la empresa.", ex);
            }

            return new EmpresaCreada
            {
                
                Nombre = empresaExistente.Nombre,
                Ruc = empresaExistente.Ruc,
                Email = empresaExistente.Email,
                
            };
        }

        public async Task<bool> BorrarEmpresaAsync(int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(empresaId));

            var empresa = await _context.Empresa.FindAsync(empresaId);
            if (empresa == null)
                throw new KeyNotFoundException($"No se encontró la empresa con ID {empresaId}.");

            try
            {
                _context.Empresa.Remove(empresa);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al borrar la empresa.", ex);
            }

            return true;
        }

        public async Task<EmpresaCreada> CrearEmpresaAsync(CrearEmpresa _empresa)
        {
            if (_empresa == null)
                throw new ArgumentNullException(nameof(_empresa));

            ValidarEmpresa(_empresa);

            if (await EmpresaExists(_empresa.Nombre, _empresa.Ruc))
                throw new InvalidOperationException("Una empresa con el mismo nombre o RUC ya existe.");

            var nuevaEmpresa = new Empresa
            {
                Nombre = _empresa.Nombre,
                Ruc = _empresa.Ruc,
                Email = _empresa.Email
                
            };

            try
            {
                _context.Empresa.Add(nuevaEmpresa);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception("Ocurrió un error al crear la empresa.", ex);
            }

            return new EmpresaCreada
            {
                Nombre = nuevaEmpresa.Nombre,
                Ruc = nuevaEmpresa.Ruc,
                Email = nuevaEmpresa.Email
            };


        }

        public Task<List<Actividad>> ObtenerActividadesPorEmpresaAsync(int empresaId)
        {
            throw new NotImplementedException();
        }

        public async Task<EmpresaCreada> ObtenerEmpresaPorIdAsync(int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.", nameof(empresaId));

            var empresa = await _context.Empresa.FindAsync(empresaId);
            if (empresa == null)
                throw new KeyNotFoundException($"No se encontró la empresa con ID {empresaId}.");

            return new EmpresaCreada
            {
                
                Nombre = empresa.Nombre,
                Ruc = empresa.Ruc,
                Email = empresa.Email
                
            };
        }

        public async Task<List<EmpresaCreada>> ObtenerTodasLasEmpresasAsync()
        {
            var empresas = await _context.Empresa.ToListAsync();
            return empresas.Select(e => new EmpresaCreada
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Ruc = e.Ruc,
                Email = e.Email
                
            }).ToList();
        }

        private void ValidarEmpresa(object empresa)
        {
            var context = new ValidationContext(empresa, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(empresa, context, results, true);

            if (!isValid)
            {
                var messages = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException(string.Join("; ", messages));
            }
        }

        private async Task<bool> EmpresaExists(string nombre, string ruc, int? empresaId = null)
        {
            return await _context.Empresa.AnyAsync(e => (e.Nombre == nombre || e.Ruc == ruc) && (!empresaId.HasValue || e.Id != empresaId.Value));
        }
    }
}
