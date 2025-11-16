using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;    // ← IMPORTANTE
using AccesoDatos.DTO;
using Datos;
using Newtonsoft.Json;

namespace API_REST_INTEGRACION.Controllers
{
    // ================================================================
    // 🔹 Controlador REST + CORS habilitado
    // ================================================================
    [EnableCors(origins: "*", headers: "*", methods: "*")]   // ← CORS ACTIVADO
    [RoutePrefix("api/integracion/autos/reservas")]
    public class IntegracionReservasController : ApiController
    {
        private readonly ReservaDatos _reservas = new ReservaDatos();
        private readonly UsuarioDatos _usuarios = new UsuarioDatos();
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();
        private readonly FacturaDatos _facturas = new FacturaDatos();

        // ================================================================
        // 🔸 GET: /api/integracion/autos/reservas/{id_reserva}
        // ================================================================
        [HttpGet]
        [Route("{id_reserva:int}")]
        public IHttpActionResult BuscarDatosReserva(int id_reserva)
        {
            try
            {
                if (id_reserva <= 0)
                    return BadRequest("El ID de la reserva no es válido.");

                var reserva = _reservas.ListarReservas()
                    .FirstOrDefault(r => r.IdReserva == id_reserva);

                if (reserva == null)
                    return NotFound();

                var vehiculo = _vehiculos.ListarVehiculos()
                    .FirstOrDefault(v => v.IdVehiculo == reserva.IdVehiculo);

                var usuario = _usuarios.ListarUsuarios()
                    .FirstOrDefault(u => u.IdUsuario == reserva.IdUsuario);

                var factura = _facturas.ListarFacturas()
                    .FirstOrDefault(f => f.IdReserva == reserva.IdReserva);

                var dto = new ReservaInfoDto
                {
                    NumeroMatricula = string.IsNullOrEmpty(vehiculo?.Matricula)
                        ? "Sin matrícula"
                        : vehiculo.Matricula,

                    Correo = usuario?.Email ?? "sin_correo@dominio.com",
                    FechaInicio = reserva.FechaInicio,
                    FechaFin = reserva.FechaFin,
                    Categoria = vehiculo?.CategoriaNombre ?? "Sin categoría",
                    Transmision = vehiculo?.TransmisionNombre ?? "No especificada",
                    ValorPagado = factura?.ValorTotal ?? reserva.Total,
                    UriFactura = factura?.UriFactura ?? "No generada aún"
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener los datos de la reserva: " + ex.Message));
            }
        }
    }

    // ================================================================
    // 🔹 DTO alineado con el bus de integración
    // ================================================================
    public class ReservaInfoDto
    {
        [JsonProperty("numero_matricula")]
        public string NumeroMatricula { get; set; }

        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("fecha_inicio")]
        public DateTime FechaInicio { get; set; }

        [JsonProperty("fecha_fin")]
        public DateTime FechaFin { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("transmision")]
        public string Transmision { get; set; }

        [JsonProperty("valor_pagado")]
        public decimal ValorPagado { get; set; }

        [JsonProperty("uri_factura")]
        public string UriFactura { get; set; }
    }
}
