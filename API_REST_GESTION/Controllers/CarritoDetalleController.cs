using System;
using System.Web.Http;
using AccesoDatos.DTO;
using Logica;

namespace API_REST_GESTION.Controllers
{
    [RoutePrefix("api/v1/carrito")]
    public class CarritoDetalleController : ApiController
    {
        private readonly CarritoLogica _logica = new CarritoLogica();

        // ============================================================
        // 🔵 OBTENER DETALLE DEL CARRITO
        // GET api/v1/carrito/{idCarrito}/detalle
        // ============================================================
        [HttpGet]
        [Route("{idCarrito}/detalle")]
        public IHttpActionResult GetDetalle(int idCarrito)
        {
            var carrito = _logica.ObtenerCarritoConItems(idCarrito);
            if (carrito == null)
                return NotFound();

            return Ok(carrito);
        }

        // ============================================================
        // 🟢 AGREGAR VEHÍCULO AL CARRITO
        // POST api/v1/carrito/agregar
        // Body: { idUsuario, idVehiculo, fechaInicio, fechaFin }
        // ============================================================
        public class AgregarVehiculoRequest
        {
            public int IdUsuario { get; set; }
            public int IdVehiculo { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
        }

        [HttpPost]
        [Route("agregar")]
        public IHttpActionResult Agregar([FromBody] AgregarVehiculoRequest req)
        {
            bool ok = _logica.AgregarVehiculo(req.IdUsuario, req.IdVehiculo, req.FechaInicio, req.FechaFin);

            if (!ok)
                return BadRequest("No se pudo agregar el vehículo al carrito.");

            return Ok(new { mensaje = "Vehículo agregado correctamente" });
        }

        // ============================================================
        // 🟠 ACTUALIZAR FECHAS DE UN ITEM
        // PUT api/v1/carrito/item/{idItem}
        // Body: { fechaInicio, fechaFin }
        // ============================================================
        public class ActualizarItemRequest
        {
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
        }

        [HttpPut]
        [Route("item/{idItem}")]
        public IHttpActionResult ActualizarItem(int idItem, [FromBody] ActualizarItemRequest req)
        {
            bool ok = _logica.ActualizarItem(idItem, req.FechaInicio, req.FechaFin);

            if (!ok)
                return BadRequest("No se pudo actualizar el item.");

            return Ok(new { mensaje = "Item actualizado correctamente" });
        }

        // ============================================================
        // 🔴 ELIMINAR ITEM
        // DELETE api/v1/carrito/item/{idItem}
        // ============================================================
        [HttpDelete]
        [Route("item/{idItem}")]
        public IHttpActionResult DeleteItem(int idItem)
        {
            bool ok = _logica.EliminarItem(idItem);

            if (!ok)
                return BadRequest("No se pudo eliminar el item.");

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
