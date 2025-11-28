using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DTO
{
    public class PagoRequestDto
    {
        public int IdReserva { get; set; }

        // Cuenta del cliente que paga
        public int CuentaCliente { get; set; }

        public int CuentaComercio { get; set; }

        // Monto a pagar
        public decimal Monto { get; set; }
    }
}
