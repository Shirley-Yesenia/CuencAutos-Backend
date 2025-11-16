using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DTO
{
    public class PagoDto
    {
        public int IdPago { get; set; }
        public int IdReserva { get; set; }
        public string Metodo { get; set; }           // Ej: 'BancoAPI', 'Tarjeta', 'Efectivo'
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string ReferenciaExterna { get; set; } // ID de transacción del banco externo
        public string Estado { get; set; }
    }
}
