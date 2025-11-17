using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DTO
{
    public class CarritoItemDto : HateoasResource
    {
        public int IdItem { get; set; }
        public int IdCarrito { get; set; }
        public int IdVehiculo { get; set; }
        public string VehiculoNombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Subtotal { get; set; }
    }
}
