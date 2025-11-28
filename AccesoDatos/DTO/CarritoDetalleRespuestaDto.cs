using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DTO
{
    public class CarritoDetalleRespuestaDto : HateoasResource
    {
        public int IdCarrito { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public List<CarritoItemDto> Items { get; set; }
    }
}
