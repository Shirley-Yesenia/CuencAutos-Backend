using AccesoDatos.DTO;
using API_REST_INTEGRACION.Hateoas.Builders;
using Datos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_REST_INTEGRACION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1/integracion/autos/reservas")]
    public class BuscarDatosController : ApiController
    {
        private readonly ReservaDatos _reservas = new ReservaDatos();
        private readonly UsuarioDatos _usuarios = new UsuarioDatos();
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();
        private readonly FacturaDatos _facturas = new FacturaDatos();

        private readonly BuscarDatosHateoas _hateoas = new BuscarDatosHateoas();

        // ================================================================
        // 🔸 GET: /api/v1/integracion/autos/reservas/{id_reserva}
        // ================================================================
        [HttpGet]
        [Route("{id_reserva:int}", Name = "BuscarDatosReserva")]
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

                // 🔗 Genera links, pero NO aparecerán en el JSON
                dto = _hateoas.GenerarLinks(dto, Url, id_reserva);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener los datos de la reserva: " + ex.Message));
            }
        }

        // Ruta usada por el HATEOAS
        [HttpGet]
        [Route("{id_reserva:int}/vehiculo", Name = "GetVehiculoPorReserva")]
        public IHttpActionResult GetVehiculoPorReserva(int id_reserva)
        {
            var reserva = _reservas.ListarReservas()
                .FirstOrDefault(r => r.IdReserva == id_reserva);

            if (reserva == null)
                return NotFound();

            var vehiculo = _vehiculos.ListarVehiculos()
                .FirstOrDefault(v => v.IdVehiculo == reserva.IdVehiculo);

            if (vehiculo == null)
                return NotFound();

            // Convertimos el Vehiculo a tu DTO limpio sin links
            var dto = new VehiculoSimpleDto
            {
                IdVehiculo = vehiculo.IdVehiculo,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Anio = vehiculo.Anio,
                IdCategoria = vehiculo.IdCategoria,
                CategoriaNombre = vehiculo.CategoriaNombre,
                IdTransmision = vehiculo.IdTransmision,
                TransmisionNombre = vehiculo.TransmisionNombre,
                Capacidad = vehiculo.Capacidad,
                PrecioDia = vehiculo.PrecioDia,
                PrecioNormal = vehiculo.PrecioNormal,
                PrecioActual = vehiculo.PrecioActual,
                Matricula = vehiculo.Matricula,
                IdSucursal = vehiculo.IdSucursal,
                SucursalNombre = vehiculo.SucursalNombre,
                UrlImagen = vehiculo.UrlImagen
            };

            return Ok(dto);
        }

    }

    // ================================================================
    // 🔹 DTO con soporte HATEOAS pero SIN mostrar Links
    // ================================================================
    public class ReservaInfoDto : HateoasResource
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

        // 🚫 Ocultar Links heredado de HateoasResource
        [JsonIgnore]
        public new IList<LinkDto> Links { get; set; }
    }
}
