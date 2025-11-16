using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DTO
{
    public class ImagenVehiculoDto
    {
        public int IdImagen { get; set; }
        public int IdVehiculo { get; set; }
        public string UriImagen { get; set; }
    }
}
