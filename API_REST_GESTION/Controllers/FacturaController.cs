using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/facturas")]
    public class FacturaController : ApiController
    {
        private readonly FacturaLogica logica = new FacturaLogica();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetFacturas()
        {
            try
            {
                var facturas = logica.ListarFacturas();
                if (facturas == null || facturas.Count == 0)
                    return NotFound();

                return Ok(facturas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{idFactura:int}")]
        public IHttpActionResult GetFacturaPorId(int idFactura)
        {
            try
            {
                var factura = logica.ObtenerFacturaPorId(idFactura);
                if (factura == null)
                    return NotFound();

                return Ok(factura);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearFactura([FromBody] FacturaDto factura)
        {
            if (factura == null)
                return BadRequest("Debe enviar los datos de la factura.");

            try
            {
                int idNuevaFactura = logica.CrearFactura(factura);
                return Ok(new { mensaje = "Factura creada correctamente.", idFactura = idNuevaFactura });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{idFactura:int}")]
        public IHttpActionResult ActualizarFactura(int idFactura, [FromBody] FacturaDto factura)
        {
            if (factura == null)
                return BadRequest("Debe enviar los datos de la factura.");

            try
            {
                factura.IdFactura = idFactura;
                bool actualizado = logica.ActualizarFactura(factura);

                if (!actualizado)
                    return BadRequest("No se pudo actualizar la factura.");

                return Ok(new { mensaje = "Factura actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{idFactura:int}")]
        public IHttpActionResult EliminarFactura(int idFactura)
        {
            try
            {
                bool eliminado = logica.EliminarFactura(idFactura);
                if (eliminado)
                    return Ok(new { mensaje = "Factura eliminada correctamente." });
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok("Servicio REST de Facturas operativo ✅");
        }
    }
}
