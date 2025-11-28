using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DTO
{
    public class PreReservaAutoRequestDto
    {
        public string IdVehiculo { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFin { get; set; }
        public int? DuracionHoldSegundos { get; set; } = 300;
    }
}