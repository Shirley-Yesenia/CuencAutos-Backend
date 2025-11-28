using AccesoDatos.DTO;
using Logica;
using System;
using System.Web.Http;
using IntegracionBanco;
using IntegracionBanco.bancoDto;

namespace API_REST_GESTION.Controllers
{
    [RoutePrefix("api/v1/pagos")]
    public class PagoController : ApiController
    {
        private readonly PagoLogica pagoLogica = new PagoLogica();

        // ===========================================================
        // GET: /api/v1/pagos  → listar todos los pagos
        // ===========================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetPagos()
        {
            try
            {
                var pagos = pagoLogica.ListarPagos();

                if (pagos == null || pagos.Count == 0)
                    return NotFound();

                return Ok(new
                {
                    data = pagos
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // GET: /api/v1/pagos/{idPago} → obtener pago por ID
        // ===========================================================
        [HttpGet]
        [Route("{idPago:int}")]
        public IHttpActionResult GetPagoPorId(int idPago)
        {
            try
            {
                var pago = pagoLogica.ObtenerPagoPorId(idPago);
                if (pago == null)
                    return NotFound();

                return Ok(new
                {
                    data = pago
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // GET: /api/v1/pagos/reserva/{idReserva} → listar pagos por reserva
        // ===========================================================
        [HttpGet]
        [Route("reserva/{idReserva:int}")]
        public IHttpActionResult GetPagosPorReserva(int idReserva)
        {
            try
            {
                var pagos = pagoLogica.ListarPagosPorReserva(idReserva);

                if (pagos == null || pagos.Count == 0)
                    return NotFound();

                return Ok(new
                {
                    data = pagos
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // POST: /api/v1/pagos → crear un pago con integración del banco
        // ===========================================================
        [HttpPost]
        [Route("")]
        public async System.Threading.Tasks.Task<IHttpActionResult> CrearPago(PagoRequestDto body)
        {
            try
            {
                if (body == null)
                    return BadRequest("Debe enviar información del pago.");

                // 1️⃣ DTO del banco
                var transaccionDto = new transaccionDto
                {
                    cuenta_origen = body.CuentaCliente,
                    cuenta_destino = body.CuentaComercio,
                    monto = body.Monto
                };

                // 2️⃣ Ejecutar transacción con el banco
                string respuestaBanco = await bancoConsumer.transaccionUnitaria(transaccionDto);
                bool aprobado = respuestaBanco == "OK";

                // 3️⃣ Crear registro interno en tabla Pago
                var pagoDto = new PagoDto
                {
                    IdReserva = body.IdReserva,
                    Metodo = "Transaccion",
                    Monto = body.Monto,
                    ReferenciaExterna = respuestaBanco,
                    Estado = aprobado ? "Exitoso" : "Fallido",
                    FechaPago = DateTime.Now
                };

                // 4️⃣ Guardar en BD
                int idPago = pagoLogica.CrearPago(pagoDto);

                return Ok(new
                {
                    message = "Pago procesado",
                    aprobado = aprobado,
                    respuestaBanco = respuestaBanco,
                    idPago = idPago
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===========================================================
        // DELETE: /api/v1/pagos/{idPago} → eliminar un pago
        // ===========================================================
        [HttpDelete]
        [Route("{idPago:int}")]
        public IHttpActionResult EliminarPago(int idPago)
        {
            try
            {
                bool eliminado = pagoLogica.EliminarPago(idPago);

                if (!eliminado)
                    return BadRequest("No se pudo eliminar el pago.");

                return Ok(new
                {
                    mensaje = "Pago eliminado correctamente."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ===========================================================
        // PUT: /api/v1/pagos/{idPago} → actualizar un pago
        // ===========================================================
        [HttpPut]
        [Route("{idPago:int}")]
        public IHttpActionResult ActualizarPago(int idPago, PagoDto body)
        {
            try
            {
                body.IdPago = idPago;

                bool actualizado = pagoLogica.ActualizarPago(body);

                if (!actualizado)
                    return BadRequest("No se pudo actualizar el pago.");

                return Ok(new
                {
                    mensaje = "Pago actualizado correctamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
