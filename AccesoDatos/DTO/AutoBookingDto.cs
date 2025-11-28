using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DTO
{
    public class AutoBookingDto 
    {
        public string IdAuto { get; set; }
        public string Tipo { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioNormal { get; set; }
        public decimal? PrecioActual { get; set; }
        public string UriImagen { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }

    }
}
