using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;      // ← IMPORTANTE PARA CORS
using AccesoDatos.DTO;
using Logica;

namespace API_REST_INTEGRACION.Controllers
{
    // HABILITAR CORS PARA ESTE CONTROLADOR
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1/integracion/autos")]
    public class IntegracionHoldController : ApiController
    {
        private readonly IntegracionAutosLogica _ln = new IntegracionAutosLogica();

        // ============================================================
        // 🔸 POST: /api/integracion/autos/hold
        // ============================================================
        [HttpPost]
        [Route("hold")]
        public IHttpActionResult CrearPreReservaAuto([FromBody] PreReservaAutoRequestDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                var resultado = _ln.CrearPreReservaAuto(request);
                return Ok(resultado);
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
