using Logica;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace API_REST.Controllers
{
    [RoutePrefix("api/reservas")]
    public class ReservaController : ApiController
    {
        private readonly ReservaLogica logica = new ReservaLogica();

        // ================================================
        // GET: /api/reservas
        // Retorna todas las reservas
        // ================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObtenerReservas()
        {
            try
            {
                var lista = logica.ListarReservas();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================
        // GET: /api/reservas/{id}
        // Retorna una reserva específica por ID
        // ================================================
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult ObtenerReservaPorId(int id)
        {
            try
            {
                var reserva = logica.ObtenerReservaPorId(id);
                if (reserva == null)
                    return NotFound();

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================
        // POST: /api/reservas
        // Crear nueva reserva
        // ================================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearReserva([FromBody] ReservaDto reservaDto)
        {
            try
            {
                if (reservaDto == null)
                    return BadRequest("Los datos de la reserva son obligatorios.");

                int nuevoId = logica.CrearReserva(reservaDto);
                var nuevaReserva = logica.ObtenerReservaPorId(nuevoId);

                return CreatedAtRoute("ReservaPorId", new { id = nuevoId }, nuevaReserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================================================
        // PUT: /api/reservas/{id}
        // Actualizar reserva existente
        // ================================================
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult ActualizarReserva(int id, [FromBody] ReservaDto reservaDto)
        {
            try
            {
                if (reservaDto == null)
                    return BadRequest("Los datos de la reserva son obligatorios.");

                reservaDto.IdReserva = id;
                bool actualizado = logica.ActualizarReserva(reservaDto);

                if (!actualizado)
                    return NotFound();

                return Ok(reservaDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================================================
        // DELETE: /api/reservas/{id}
        // Eliminar reserva
        // ================================================
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult EliminarReserva(int id)
        {
            try
            {
                bool eliminado = logica.EliminarReserva(id);
                if (!eliminado)
                    return NotFound();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
