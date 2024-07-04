using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Infraestructure.EFDataContext.Context
{
    public class DBContext : IdentityDbContext<Usuario>
    {
        public DBContext( DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<RefrescarToken> RefrescarToken { get; set; }

        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Actividad> Actividad { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Resenia> Resenia { get; set; }
        public DbSet<Itinerario> Itinerario { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Promocion> Promociones { get; set; }

    }
}
