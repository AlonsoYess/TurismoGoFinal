using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Domain.Entities.Entidades;

namespace Turismo.Services.Interfaces.Responses
{
    public class ActividadCreada
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Empresa { get; set; }


        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string Destino { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public decimal Precio { get; set; }
        public int Capacidad { get; set; }
        public string Imagen {  get; set; }

        public string ImagenURL {  get; set; }
        
        public List<ReservaCreada> Reservas { get; set; }
        public List<ReseniaCreada> Resenia { get; set; }
        public List<ItinerarioCreado> Itinerarios { get; set; }

        public double PromedioCalificacion { get; set; }

        public int TotalCalificaciones { get; set; }
    }
}
