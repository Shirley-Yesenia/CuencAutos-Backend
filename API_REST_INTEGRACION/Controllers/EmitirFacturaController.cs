using System;
using System.Web.Http;
using System.Web.Http.Cors;
using API_REST_INTEGRACION.Hateoas.Builders;
using AccesoDatos.DTO;

namespace API_REST_INTEGRACION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmitirFacturaController : ApiController
    {
        // ================================================================
        // 🔹 POST: /api/v1/integracion/autos/invoices
        // ================================================================
        [HttpPost]
        [Route("api/v1/integracion/autos/invoices", Name = "EmitirFactura")]
        public IHttpActionResult EmitirFactura([FromBody] FacturaOrquestadorDto dto)
        {
            if (dto == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(dto.id_reserva))
                return BadRequest("Debe especificarse el ID de la reserva.");

            if (string.IsNullOrWhiteSpace(dto.correo))
                return BadRequest("Debe especificarse el correo del cliente.");

            if (string.IsNullOrWhiteSpace(dto.nombre))
                return BadRequest("Debe especificarse el nombre del cliente.");

            if (string.IsNullOrWhiteSpace(dto.tipo_identificacion))
                return BadRequest("Debe especificarse el tipo de identificación.");

            if (string.IsNullOrWhiteSpace(dto.identificacion))
                return BadRequest("Debe especificarse la identificación.");

            if (dto.valor <= 0)
                return BadRequest("El valor de la factura debe ser mayor que cero.");

            try
            {
                // 🧾 Generar ID de factura aleatorio
                var idFactura = "FAC-" + new Random().Next(1000, 9999);

                // 📄 URL simulada de la factura
                var urlFactura = $"https://cuencautosinte.runasp.net/facturas/{idFactura}.pdf";

                // DTO de respuesta
                var respuesta = new
                {
                    id_factura = idFactura,
                    url_factura = urlFactura,
                    estado = "Factura emitida correctamente"
                };

                // 🔗 Generar HATEOAS dinámico
                var hateoas = new EmitirFacturaHateoas(Url, idFactura, dto.id_reserva);

                return Ok(new
                {
                    datos = respuesta,
                    links = hateoas.GetLinks()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al emitir la factura: " + ex.Message));
            }
        }
    }

    // DTO requerido — SIN LINKS
    public class FacturaOrquestadorDto
    {
        public string id_reserva { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }
        public string tipo_identificacion { get; set; }
        public string identificacion { get; set; }
        public decimal valor { get; set; }
    }
}
