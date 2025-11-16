using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/reservas")]
    public class ReservaController : ApiController
    {
        private readonly ReservaLogica logica = new ReservaLogica();

        [HttpGet]
        [Route("")]
        public IHttpActionResult ObtenerReservas()
        {
            try
            {
                var lista = logica.ListarReservas();
                if (lista == null || lista.Count == 0)
                    return NotFound();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{idReserva:int}")]
        public IHttpActionResult ObtenerReservaPorId(int idReserva)
        {
            try
            {
                var reserva = logica.ObtenerReservaPorId(idReserva);
                if (reserva == null)
                    return NotFound();

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("usuario/{idUsuario:int}")]
        public IHttpActionResult ObtenerReservasPorUsuario(int idUsuario)
        {
            try
            {
                var reservas = logica.ListarReservasPorUsuario(idUsuario);
                if (reservas == null || reservas.Count == 0)
                    return NotFound();

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearReserva([FromBody] ReservaDto reserva)
        {
            if (reserva == null)
                return BadRequest("Debe enviar los datos de la reserva.");

            try
            {
                int idGenerado = logica.CrearReserva(reserva);
                if (idGenerado <= 0)
                    return BadRequest("No se pudo crear la reserva.");

                return Ok(new { mensaje = "Reserva creada correctamente.", idReserva = idGenerado });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{idReserva:int}")]
        public IHttpActionResult ActualizarReserva(int idReserva, [FromBody] ReservaDto reserva)
        {
            if (reserva == null)
                return BadRequest("Debe enviar los datos de la reserva.");

            try
            {
                reserva.IdReserva = idReserva;
                bool actualizado = logica.ActualizarReserva(reserva);

                if (!actualizado)
                    return BadRequest("No se pudo actualizar la reserva.");

                return Ok(new { mensaje = "Reserva actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{idReserva:int}")]
        public IHttpActionResult EliminarReserva(int idReserva)
        {
            try
            {
                bool eliminado = logica.EliminarReserva(idReserva);
                if (!eliminado)
                    return NotFound();

                return Ok(new { mensaje = "Reserva eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPatch]
        [Route("{idReserva:int}/estado/{nuevoEstado}")]
        public IHttpActionResult CambiarEstadoReserva(int idReserva, string nuevoEstado)
        {
            try
            {
                bool cambiado = logica.CambiarEstadoReserva(idReserva, nuevoEstado);
                if (!cambiado)
                    return BadRequest("No se pudo cambiar el estado.");

                return Ok(new { mensaje = $"Estado de la reserva {idReserva} actualizado a '{nuevoEstado}'." });
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
            return Ok("Servicio REST de Reservas operativo ✅");
        }
    }
}
