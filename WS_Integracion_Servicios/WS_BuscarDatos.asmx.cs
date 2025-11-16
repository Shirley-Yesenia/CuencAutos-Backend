using System;
using System.Linq;
using System.Web.Services;
using AccesoDatos.DTO;
using Datos;

namespace WS_Integracion_Servicios
{
    /// <summary>
    /// Servicio SOAP para obtener toda la información de una reserva de autos por su ID.
    /// Equivalente al endpoint REST /api/integracion/autos/reservas/{id_reserva}.
    /// </summary>
    [WebService(Namespace = "http://ws.rentaautos.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_BuscarDatos : WebService
    {
        // 🔹 Dependencias de acceso a datos
        private readonly ReservaDatos _reservas = new ReservaDatos();
        private readonly UsuarioDatos _usuarios = new UsuarioDatos();
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();
        private readonly FacturaDatos _facturas = new FacturaDatos();

        /// <summary>
        /// Devuelve todos los datos necesarios para generar la factura de una reserva.
        /// Incluye información del vehículo, cliente, categoría y factura.
        /// </summary>
        /// <param name="id_reserva">ID de la reserva a consultar.</param>
        /// <returns>Objeto con los datos de la reserva o null si no existe.</returns>
        [WebMethod(Description = "Devuelve los datos completos de una reserva de autos para integración con facturación.")]
        public ReservaInfoSoapDto BuscarDatosReserva(int id_reserva)
        {
            try
            {
                // Validar ID
                if (id_reserva <= 0)
                    throw new ArgumentException("El ID de la reserva no es válido.");

                // 🔹 Buscar reserva
                var reserva = _reservas.ListarReservas()
                    .FirstOrDefault(r => r.IdReserva == id_reserva);
                if (reserva == null)
                    return null;

                // 🔹 Buscar vehículo
                var vehiculo = _vehiculos.ListarVehiculos()
                    .FirstOrDefault(v => v.IdVehiculo == reserva.IdVehiculo);

                // 🔹 Buscar usuario
                var usuario = _usuarios.ListarUsuarios()
                    .FirstOrDefault(u => u.IdUsuario == reserva.IdUsuario);

                // 🔹 Buscar factura
                var factura = _facturas.ListarFacturas()
                    .FirstOrDefault(f => f.IdReserva == reserva.IdReserva);

                // ✅ Construir DTO final
                return new ReservaInfoSoapDto
                {
                    numero_matricula = string.IsNullOrEmpty(vehiculo?.Matricula)
                        ? "Sin matrícula"
                        : vehiculo.Matricula,

                    correo = usuario?.Email ?? "sin_correo@dominio.com",
                    fecha_inicio = reserva.FechaInicio,
                    fecha_fin = reserva.FechaFin,
                    categoria = vehiculo?.CategoriaNombre ?? "Sin categoría",
                    transmision = vehiculo?.TransmisionNombre ?? "No especificada",
                    valor_pagado = factura?.ValorTotal ?? reserva.Total,
                    uri_factura = factura?.UriFactura ?? "No generada aún"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos de la reserva: " + ex.Message);
            }
        }
    }

    /// <summary>
    /// DTO de salida alineado con el contrato de integración (versión SOAP)
    /// </summary>
    public class ReservaInfoSoapDto
    {
        public string numero_matricula { get; set; }
        public string correo { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public string categoria { get; set; }
        public string transmision { get; set; }
        public decimal valor_pagado { get; set; }
        public string uri_factura { get; set; }
    }
}
