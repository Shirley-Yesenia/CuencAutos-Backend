using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;  // ← IMPORTANTE PARA CORS
using AccesoDatos.DTO;
using Logica;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]   // ← CORS ACTIVADO
    [RoutePrefix("api/bloqueosvehiculos")]
    public class BloqueosVehiculosController : ApiController
    {
        private readonly BloqueoVehiculoLogica logica = new BloqueoVehiculoLogica();

        public BloqueosVehiculosController()
        {
            // ⏱ Tiempo máximo de espera: 600 segundos
            System.Web.HttpContext.Current.Server.ScriptTimeout = 600;
        }

        // ============================================================
        // 🔵 GET: api/bloqueosvehiculos/vehiculo/{idVehiculo}
        // ============================================================
        [HttpGet]
        [Route("vehiculo/{idVehiculo:int}")]
        public IHttpActionResult GetBloqueosPorVehiculo(int idVehiculo)
        {
            try
            {
                var bloqueos = logica.ListarBloqueosPorVehiculo(idVehiculo);
                if (bloqueos == null || bloqueos.Count == 0)
                    return NotFound();

                return Ok(bloqueos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // 🟢 POST: api/bloqueosvehiculos
        // ============================================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] BloqueoVehiculoDto bloqueo)
        {
            if (bloqueo == null)
                return BadRequest("El objeto 'bloqueo' no puede ser nulo.");

            try
            {
                bool creado = logica.CrearBloqueo(bloqueo);
                if (creado)
                    return Ok(new { mensaje = "Bloqueo creado correctamente." });
                else
                    return BadRequest("No se pudo crear el bloqueo.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // 🔴 DELETE: api/bloqueosvehiculos/{idHold}
        // ============================================================
        [HttpDelete]
        [Route("{idHold:int}")]
        public IHttpActionResult Delete(int idHold)
        {
            try
            {
                bool eliminado = logica.EliminarBloqueo(idHold);
                if (eliminado)
                    return Ok(new { mensaje = "Bloqueo eliminado correctamente." });
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // GET: api/bloqueosvehiculos (prueba)
        // ============================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok("Servicio REST de BloqueosVehiculos operativo ✅");
        }
    }
}
