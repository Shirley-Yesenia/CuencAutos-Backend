using System;
using System.Collections.Generic;

namespace AccesoDatos.DTO
{
    public class FacturaDto
    {
        public int IdFactura { get; set; }
        public int IdReserva { get; set; }

        // 🔹 Agregamos este campo (necesario para integración REST)
        public int IdUsuario { get; set; }

        public string UriFactura { get; set; }     // Enlace o ruta del PDF generado
        public DateTime? FechaEmision { get; set; }
        public decimal ValorTotal { get; set; }
        public string Descripcion { get; set; }    // Descripción general de la factura

        public List<DetalleFacturaDto> Detalles { get; set; } = new List<DetalleFacturaDto>();
    }
}
