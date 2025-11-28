using AccesoDatos.DTO;
using API_REST_INTEGRACION.Hateoas.Builders;
using Datos;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_REST_INTEGRACION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValidarDisponibilidadController : ApiController
    {
        private readonly ReservaDatos _reservas = new ReservaDatos();

        // ================================================================
        // 🔹 POST: /api/v1/integracion/autos/availability
        // ================================================================
        [HttpPost]
        [Route("api/v1/integracion/autos/availability")]
        public IHttpActionResult ValidarDisponibilidad([FromBody] ValidarDisponibilidadDto dto)
        {
            if (dto == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            // ⭐ Validar que IdVehiculo sea numérico
            if (!int.TryParse(dto.IdVehiculo, out int idVehiculoInt))
                return BadRequest("El IdVehiculo debe ser numérico.");

            // Lógica de negocio
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
