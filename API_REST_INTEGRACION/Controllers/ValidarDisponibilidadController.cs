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
        // 🔹 POST: /api/integracion/autos/availability
        // ================================================================
        [HttpPost]
        [Route("api/integracion/autos/availability")]
        public IHttpActionResult ValidarDisponibilidad([FromBody] ValidarDisponibilidadDto dto)
        {
            if (dto == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            bool disponible = _reservas.ValidarDisponibilidad(dto.IdVehiculo, dto.FechaInicio, dto.FechaFin);

            return Ok(new
            {
                dto.IdVehiculo,
                dto.FechaInicio,
                dto.FechaFin,
                Disponible = disponible,
                Mensaje = disponible ? "Vehículo disponible ✅" : "No disponible ❌"
            });
        }
    }

    public class ValidarDisponibilidadDto
    {
        public int IdVehiculo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
