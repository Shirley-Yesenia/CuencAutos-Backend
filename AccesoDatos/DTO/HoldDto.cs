using System;

namespace AccesoDatos.DTO
{
    public class HoldDto
    {
        public int IdHold { get; set; }
        public int IdUsuario { get; set; }
        public int IdVehiculo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public decimal MontoBloqueado { get; set; }
        public string ReferenciaBanco { get; set; }
        public string Estado { get; set; }
    }
}
