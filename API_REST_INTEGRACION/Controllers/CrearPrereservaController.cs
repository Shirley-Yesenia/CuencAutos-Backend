using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using AccesoDatos.DTO;
using Logica;
using API_REST_INTEGRACION.Hateoas.Builders;

namespace API_REST_INTEGRACION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1/integracion/autos")]
    public class CrearPrereservaController : ApiController
    {
        private readonly IntegracionAutosLogica _ln = new IntegracionAutosLogica();

        // ============================================================
        // 🔸 POST: /api/v1/integracion/autos/hold
        // ============================================================
        [HttpPost]
        [Route("hold", Name = "CrearPreReservaAuto")]
        public IHttpActionResult CrearPreReservaAuto([FromBody] PreReservaAutoRequestDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                var resultado = _ln.CrearPreReservaAuto(request);

                var hateoas = new CrearPrereservaHateoas(Url, resultado.IdHold);

                var response = new
                {
                    datos = resultado,
                    _links = hateoas.GetLinks()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    mensaje = "Error al crear la pre-reserva.",
                    detalle = ex.Message
                });
            }
        }
    }
}
