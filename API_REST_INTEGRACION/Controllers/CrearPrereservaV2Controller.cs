using Datos;
using System;
using System.Web.Http;
using System.Web.Http.Cors;
using AccesoDatos.DTO;
using API_REST_INTEGRACION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v2/prereserva")]
    public class PreReservaV2Controller : ApiController
    {
        private readonly PreReservaDatos _datos = new PreReservaDatos();

        // POST api/v2/prereserva/auto
        [HttpPost]
        [Route("auto", Name = "CrearPreReservaAutoV2")]
        public IHttpActionResult CrearPreReservaAuto([FromBody] PreReservaAutoV2RequestDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                // 1. Validar vehículo existente
                if (!_datos.ExisteVehiculo(request.IdVehiculo.ToString()))
                    return BadRequest("El vehículo no existe.");

                // 2. Validar hold activo que choque con fechas
                if (_datos.ExisteHoldActivo(request.IdVehiculo, request.FechaInicio, request.FechaFin))
                    return BadRequest("El vehículo ya tiene un hold activo en ese rango de fechas.");

                // 3. Crear hold
                var idHold = _datos.CrearHold(
                    request.IdVehiculo,
                    request.FechaInicio,
                    request.FechaFin,
                    request.DuracionHoldSegundos
                );

                // 4. Construir HATEOAS exactamente como antes
                var hateoas = new CrearPrereservaHateoas(Url, idHold.ToString());

                // 5. Respuesta final con HATEOAS (igual al estilo antiguo)
                return Ok(new
                {
                    id_hold = idHold,
                    mensaje = "Prereserva creada correctamente",
                    expiracion = DateTime.Now.AddSeconds(request.DuracionHoldSegundos),
                    _links = hateoas.GetLinks()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
