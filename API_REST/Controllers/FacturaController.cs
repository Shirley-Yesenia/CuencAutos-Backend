using System;
using System.Collections.Generic;
using System.Web.Http;
using AccesoDatos.DTO;
using Logica;

namespace API_REST.Controllers
{
    /// <summary>
    /// API REST para la gestión de facturas (versión equivalente al WS_Factura SOAP).
    /// </summary>
    [RoutePrefix("api/facturas")]
    public class FacturaRestController : ApiController
    {
        private readonly FacturaLogica _logica = new FacturaLogica();

        // ============================================================
        // 🔵 GET /api/facturas
        // ============================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObtenerFacturas()
        {
            try
            {
                var facturas = _logica.ListarFacturas();
                return Ok(facturas);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener las facturas: " + ex.Message));
            }
        }

        // ============================================================
        // 🔍 GET /api/facturas/{idFactura}
        // ============================================================
        [HttpGet]
        [Route("{idFactura:int}")]
        public IHttpActionResult ObtenerFacturaPorId(int idFactura)
        {
            try
            {
                var factura = _logica.ObtenerFacturaPorId(idFactura);
                if (factura == null)
                    return NotFound();

                return Ok(factura);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener la factura: " + ex.Message));
            }
        }

        // ============================================================
        // 🟢 POST /api/facturas
        // ============================================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearFactura([FromBody] FacturaDto factura)
        {
            try
            {
                if (factura == null)
                    return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

                int id = _logica.CrearFactura(factura);
                return Ok(new { IdFactura = id, Mensaje = "Factura creada correctamente ✅" });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al crear la factura: " + ex.Message));
            }
        }

        // ============================================================
        // 🟠 PUT /api/facturas/{idFactura}
        // ============================================================
        [HttpPut]
        [Route("{idFactura:int}")]
        public IHttpActionResult ActualizarFactura(int idFactura, [FromBody] FacturaDto factura)
        {
            try
            {
                if (factura == null)
                    return BadRequest("Debe proporcionar los datos de la factura.");

                factura.IdFactura = idFactura;
                bool actualizado = _logica.ActualizarFactura(factura);

                if (!actualizado)
                    return NotFound();

                return Ok(new { Mensaje = "Factura actualizada correctamente ✅" });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al actualizar la factura: " + ex.Message));
            }
        }

        // ============================================================
        // 🔴 DELETE /api/facturas/{idFactura}
        // ============================================================
        [HttpDelete]
        [Route("{idFactura:int}")]
        public IHttpActionResult EliminarFactura(int idFactura)
        {
            try
            {
                bool eliminado = _logica.EliminarFactura(idFactura);
                if (!eliminado)
                    return NotFound();

                return Ok(new { Mensaje = "Factura eliminada correctamente ✅" });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al eliminar la factura: " + ex.Message));
            }
        }
    }
}
