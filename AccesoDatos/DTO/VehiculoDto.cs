using System;

namespace AccesoDatos.DTO
{
    public class VehiculoDto
    {
        public int IdVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }

        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; }

        public int IdTransmision { get; set; }
        public string TransmisionNombre { get; set; }  // ✅ mantener sólo este (el otro sobra)

        public int Capacidad { get; set; }

        // 💰 Campos de precios
        public decimal PrecioDia { get; set; }
        public decimal PrecioNormal { get; set; }
        public decimal? PrecioActual { get; set; }

        // 🪪 Identificación y detalles extra
        public string Matricula { get; set; }           // ✅ ahora se llenará correctamente

        // 🏷️ Relación con promoción
        public int? IdPromocion { get; set; }
        public string PromocionNombre { get; set; }
        public decimal? PorcentajeDescuento { get; set; }

        // 📦 Estado y ubicación
        public string Estado { get; set; }
        public string Descripcion { get; set; }

        public int IdSucursal { get; set; }
        public string SucursalNombre { get; set; }

        // 🖼️ Imagen principal
        public string UrlImagen { get; set; }
    }
}
