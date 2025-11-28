using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    
    [RoutePrefix("api/v1/facturas")]
    public class FacturaController : ApiController
    {
        private readonly FacturaLogica logica = new FacturaLogica();
        private readonly FacturaHateoas hateoas;

        public FacturaController()
        {
            var baseUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            hateoas = new FacturaHateoas(baseUrl);
        }

        // ===========================================================
        // GET: /api/v1/facturas
        // ===========================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetFacturas()
        {
            try
            {
                var facturas = logica.ListarFacturas();
                if (facturas == null || facturas.Count == 0)
                    return NotFound();

                return Ok(new
                {
                    data = facturas,
                    _links = hateoas.ConstruirLinksColeccion()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // GET: /api/v1/facturas/{id}
        // ===========================================================
        [HttpGet]
        [Route("{idFactura:int}")]
        public IHttpActionResult GetFacturaPorId(int idFactura)
        {
            try
            {
                var factura = logica.ObtenerFacturaPorId(idFactura);
                if (factura == null)
                    return NotFound();

                return Ok(new
                {
                    data = factura,
                    _links = hateoas.ConstruirLinksFactura(idFactura)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // POST: /api/v1/facturas
        // ===========================================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearFactura([FromBody] FacturaDto factura)
        {
            if (factura == null)
                return BadRequest("Debe enviar los datos de la factura.");

            try
            {
                int idNuevaFactura = logica.CrearFactura(factura);

                return Ok(new
                {
                    mensaje = "Factura creada correctamente.",
                    idFactura = idNuevaFactura,
                    _links = hateoas.ConstruirLinksFactura(idNuevaFactura)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // PUT: /api/v1/facturas/{id}
        // ===========================================================
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

                return Ok(new
                {
                    mensaje = "Factura actualizada correctamente.",
                    _links = hateoas.ConstruirLinksFactura(idFactura)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // DELETE: /api/v1/facturas/{id}
        // ===========================================================
        [HttpDelete]
        [Route("{idFactura:int}")]
        public IHttpActionResult EliminarFactura(int idFactura)
        {
            try
            {
                bool eliminado = logica.EliminarFactura(idFactura);
                if (!eliminado)
                    return NotFound();

                return Ok(new
                {
                    mensaje = "Factura eliminada correctamente.",
                    _links = hateoas.ConstruirLinksColeccion()
                });
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
