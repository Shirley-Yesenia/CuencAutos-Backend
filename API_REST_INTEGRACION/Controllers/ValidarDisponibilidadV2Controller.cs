using AccesoDatos;
using AccesoDatos.DTO;
using API_REST_INTEGRACION.Hateoas.Builders;
using Datos;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_REST_INTEGRACION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValidarDisponibilidadV2Controller : ApiController
    {
        private readonly ReservaDatos _reservas = new ReservaDatos();
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();

        // ================================================================
        // 🔹 POST: /api/v1/integracion/autos/availability
        // ================================================================
        [HttpPost]
        [Route("api/v2/integracion/autos/availability")]
        public IHttpActionResult ValidarDisponibilidad([FromBody] ValidarDisponibilidadDto dto)
        {
            if (dto == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            if (!int.TryParse(dto.IdVehiculo, out int idVehiculoInt))
                return BadRequest("El IdVehiculo debe ser numérico.");

            // ⭐ Validar existencia del vehículo
            var vehiculo = _vehiculos.ObtenerPorId(idVehiculoInt);
            if (vehiculo == null)
                return NotFound(); // o BadRequest("El vehículo no existe")

            // ⭐ Lógica original (no se toca)
            bool disponible = _reservas.ValidarDisponibilidad(idVehiculoInt, dto.FechaInicio, dto.FechaFin);

            var respuesta = new
            {
                IdVehiculo = idVehiculoInt,
                dto.FechaInicio,
                dto.FechaFin,
                Disponible = disponible,
                Mensaje = disponible ? "Vehículo disponible ✅" : "No disponible ❌",
                _links = new ValidarDisponibilidadHateoas().Build(idVehiculoInt)
            };

            return Ok(respuesta);
        }

    }
}
