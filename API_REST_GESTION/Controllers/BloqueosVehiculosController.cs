using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;
using AccesoDatos.DTO;
using Logica;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1/bloqueosvehiculos")]
    public class BloqueosVehiculosController : ApiController
    {
        private readonly BloqueoVehiculoLogica logica = new BloqueoVehiculoLogica();

        public BloqueosVehiculosController()
        {
            System.Web.HttpContext.Current.Server.ScriptTimeout = 600;
        }

        // --------------------------------------------------------------
        // HATEOAS BUILDER
        // --------------------------------------------------------------
        private BloqueoVehiculosHateoas GetBuilder()
        {
            return new BloqueoVehiculosHateoas(new UrlHelper(Request));
        }

        // ============================================================
        // 🔵 GET: api/v1/bloqueosvehiculos/vehiculo/{idVehiculo}
        // ============================================================
        [HttpGet]
        [Route("vehiculo/{idVehiculo:int}", Name = "GetBloqueosPorVehiculo")]
        public IHttpActionResult GetBloqueosPorVehiculo(int idVehiculo)
        {
            try
            {
                var bloqueos = logica.ListarBloqueosPorVehiculo(idVehiculo);
                if (bloqueos == null || bloqueos.Count == 0)
                    return NotFound();

                var builder = GetBuilder();
                var resultado = new List<object>();

                foreach (var b in bloqueos)
                    resultado.Add(builder.Build(b));

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // 🟢 POST: api/v1/bloqueosvehiculos
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

                if (!creado)
                    return BadRequest("No se pudo crear el bloqueo.");

                var builder = GetBuilder();
                return Ok(builder.Build(bloqueo));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // 🔴 DELETE: api/v1/bloqueosvehiculos/{idHold}
        // ============================================================
        [HttpDelete]
        [Route("{idHold:int}", Name = "DeleteBloqueo")]
        public IHttpActionResult Delete(int idHold)
        {
            try
            {
                bool eliminado = logica.EliminarBloqueo(idHold);
                if (!eliminado)
                    return NotFound();

                return Ok(new { mensaje = "Bloqueo eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // GET: api/v1/bloqueosvehiculos
        // ============================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok("Servicio REST de BloqueosVehiculos operativo ✅");
        }

        // ============================================================
        // GET: api/v1/bloqueosvehiculos/{idHold}
        // ============================================================
        [HttpGet]
        [Route("{idHold:int}", Name = "GetBloqueoById")]
        public IHttpActionResult GetById(int idHold)
        {
            try
            {
                var bloqueo = logica.ObtenerBloqueo(idHold);
                if (bloqueo == null)
                    return NotFound();

                var builder = GetBuilder();
                return Ok(builder.Build(bloqueo));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
